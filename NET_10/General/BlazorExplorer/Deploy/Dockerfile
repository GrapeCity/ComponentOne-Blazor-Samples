# ==========================================
# Stage 1: Build the Blazor WASM app
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Install Python for WASM AOT
RUN apt-get update && \
    apt-get install -y python3 python3-pip && \
    ln -s /usr/bin/python3 /usr/bin/python && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Install Blazor WASM workload
RUN dotnet workload install wasm-tools

# Copy and restore
# Copy solution and project files
COPY ["Server/BlazorExplorer.Server.csproj", "Server/"]
COPY ["Client/BlazorExplorer.Client.csproj", "Client/"]
COPY ["Shared/BlazorExplorer.Shared.csproj", "Shared/"]
RUN dotnet restore "Server/BlazorExplorer.Server.csproj"

# Copy all source and build
COPY . .

# Ensure license file is in place
RUN mkdir -p /src/Client/obj/Release/net10.0 && \
    mkdir -p /src/Server/obj/Release/net10.0 && \
    cp /src/Client/.sa3licx /src/Client/obj/Release/net10.0/.sa3licx 2>/dev/null || true && \
    cp /src/Server/.sa3licx /src/Server/obj/Release/net10.0/.sa3licx 2>/dev/null || true

# Publish
WORKDIR "/src/Server"
RUN dotnet build "BlazorExplorer.Server.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "BlazorExplorer.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:RollForward=LatestMinor

# ==========================================
# Stage 2: Runtime — Nginx with dynamic BASE_PATH
# ==========================================
FROM nginx:alpine

# Copy static assets
COPY --from=publish /app/publish/wwwroot /usr/share/nginx/html

# Copy startup script
COPY entrypoint.sh /entrypoint.sh
RUN chmod +x /entrypoint.sh

EXPOSE 5000

ENTRYPOINT ["Script/entrypoint.sh"]