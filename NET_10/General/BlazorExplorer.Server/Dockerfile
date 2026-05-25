# Use the official .NET 10 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the project file and license file first
COPY ["BlazorExplorer.Server.csproj", "./"]
COPY [".sa3licx", "./"]
COPY ["runtimeconfig.template.json", "./"]

# Restore dependencies
RUN dotnet restore "BlazorExplorer.Server.csproj"

# Copy the rest of the source code
COPY . .

RUN rm -rf bin/ obj/

# Ensure .sa3licx is in the obj directories for both Debug and Release builds
RUN mkdir -p /src/obj/Release/net10.0 && \
    mkdir -p /src/obj/Debug/net10.0 && \
    cp /src/.sa3licx /src/obj/Release/net10.0/.sa3licx 2>/dev/null || true && \
    cp /src/.sa3licx /src/obj/Debug/net10.0/.sa3licx 2>/dev/null || true

# Publish the application (which includes build)
# Explicitly include wwwroot in the publish
RUN dotnet publish "BlazorExplorer.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Verify the license file and published content
RUN ls -la /src/.sa3licx && echo "License file exists in source" || echo "License file NOT found"
RUN ls -la /app/publish/ | head -20
RUN ls -la /app/publish/wwwroot/Content/css/images/icons/ || echo "Icons directory not found"

# Use the official .NET 10 ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Install libgdiplus, fonts, and required libraries for System.Drawing support on Linux
RUN apt-get update && \
    apt-get install -y \
    libgdiplus \
    libc6-dev \
    fontconfig \
    fonts-dejavu-core \
    fonts-liberation && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Expose port 4000
EXPOSE 4000

# Copy the published application from the build stage
COPY --from=build /app/publish .

# Copy the license file to the runtime directory as well (for runtime access)
COPY --from=build /src/.sa3licx ./.sa3licx

# Set the environment variable to listen on port 4000
ENV ASPNETCORE_URLS=http://+:4000

# Enable System.Drawing on non-Windows platforms
ENV System__Drawing__EnableUnixSupport=true

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "BlazorExplorer.dll"]