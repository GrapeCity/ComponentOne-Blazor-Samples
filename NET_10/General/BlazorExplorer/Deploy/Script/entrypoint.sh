#!/bin/sh
set -e

# 1. Read BASE_PATH from runtime env, default to root
BASE_PATH="${BASE_PATH:-/}"

# 2. Normalize: must start with '/' and end with '/'
case "$BASE_PATH" in
    /*) ;;
    *) BASE_PATH="/$BASE_PATH" ;;
esac
case "$BASE_PATH" in
    */) ;;
    *) BASE_PATH="$BASE_PATH/" ;;
esac

echo "Runtime BASE_PATH: $BASE_PATH"

# 3. Patch index.html <base href> at runtime
sed -i "s|<base href=\"[^\"]*\"|<base href=\"${BASE_PATH}\"|g" /usr/share/nginx/html/index.html

# 4. Generate Nginx config dynamically
if [ "$BASE_PATH" = "/" ]; then
    cat > /etc/nginx/conf.d/default.conf <<'EOF'
server {
    listen 5000;
    gzip on;
    gzip_types text/plain application/javascript application/x-javascript text/javascript text/xml text/css application/wasm;

    location / {
        root /usr/share/nginx/html;
        try_files $uri $uri/ /index.html =404;
    }
}
EOF
else
    # We use a quoted heredoc 'EOF' to protect $uri, $scheme, and $http_host from shell expansion.
    # Then we use sed to replace the placeholder __BASE_PATH__ with our actual variable.
    cat > /etc/nginx/conf.d/default.conf.tmp <<'EOF'
server {
    listen 5000;
    gzip on;
    gzip_types text/plain application/javascript application/x-javascript text/javascript text/xml text/css application/wasm;

    location __BASE_PATH__ {
        alias /usr/share/nginx/html/;
        try_files $uri $uri/ __BASE_PATH__index.html =404;
    }

    location / {
        return 301 $scheme://$http_host__BASE_PATH__;
    }
}
EOF

    # Dynamically inject the actual BASE_PATH into the template
    sed "s|__BASE_PATH__|${BASE_PATH}|g" /etc/nginx/conf.d/default.conf.tmp > /etc/nginx/conf.d/default.conf
    rm /etc/nginx/conf.d/default.conf.tmp
fi

echo "Nginx config generated."

# 5. Start Nginx
exec nginx -g 'daemon off;'