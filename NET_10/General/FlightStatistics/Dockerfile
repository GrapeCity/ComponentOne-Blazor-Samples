# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Install Python (required for WASM AOT compilation) and other dependencies
RUN apt-get update && \
    apt-get install -y python3 python3-pip file && \
    ln -s /usr/bin/python3 /usr/bin/python && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Install required workload for Blazor WebAssembly
RUN dotnet workload install wasm-tools

# Copy solution and project files
COPY ["Server/FlightStatistics.Server.csproj", "Server/"]
COPY ["Client/FlightStatistics.Client.csproj", "Client/"]
COPY ["Shared/FlightStatistics.Shared.csproj", "Shared/"]

# Restore dependencies
RUN dotnet restore "Server/FlightStatistics.Server.csproj"

# Copy all source files
COPY . .

# Verify source database exists and show its structure
RUN echo "=== Checking source database ===" && \
    ls -lh /src/Server/App_Data/ && \
    file /src/Server/App_Data/FlightStatistics.db

# Ensure .sa3licx is in the obj directories before build
RUN mkdir -p /src/Client/obj/Release/net10.0 && \
    mkdir -p /src/Server/obj/Release/net10.0 && \
    cp /src/Client/.sa3licx /src/Client/obj/Release/net10.0/.sa3licx 2>/dev/null || true && \
    cp /src/Server/.sa3licx /src/Server/obj/Release/net10.0/.sa3licx 2>/dev/null || true

# Build the Server project (which includes the Client WASM)
WORKDIR "/src/Server"
RUN dotnet build "FlightStatistics.Server.csproj" -c Release -o /app/build

# Stage 2: Publish the application
FROM build AS publish
RUN dotnet publish "FlightStatistics.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:RollForward=LatestMinor

# Verify the published content
RUN echo "=== Checking published App_Data directory ===" && \
    ls -la /app/publish/App_Data/ 2>/dev/null || echo "App_Data directory not in publish output"

# Stage 3: Create runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Install SQLite libraries and tools
RUN apt-get update && \
    apt-get install -y \
    libsqlite3-0 \
    libsqlite3-dev \
    sqlite3 && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Copy published files
COPY --from=publish /app/publish .

# Create App_Data directory
RUN mkdir -p /app/App_Data

# Copy the database file from source (not publish, as it may not be included)
COPY --from=build /src/Server/App_Data/FlightStatistics.db ./App_Data/FlightStatistics.db

# Verify final App_Data directory contents and database structure
RUN echo "=== Final App_Data directory contents ===" && \
    ls -lh /app/App_Data/ && \
    echo "=== Database tables ===" && \
    sqlite3 /app/App_Data/FlightStatistics.db ".tables"

# Ensure proper permissions for App_Data directory (SQLite needs write access)
RUN chmod -R 777 /app/App_Data

# Expose port 3000
EXPOSE 3000

# Set environment variables
ENV ASPNETCORE_URLS=http://+:3000
ENV ASPNETCORE_ENVIRONMENT=Production

# Set the entry point
ENTRYPOINT ["dotnet", "FlightStatistics.Server.dll"]
