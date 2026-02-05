# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the entire solution structure
COPY . .

# Restore dependencies for the main API project
RUN dotnet restore "residence.api/residence.api.csproj" --verbosity diagnostic

# Build the solution
RUN dotnet build "residence.api/residence.api.csproj" \
    -c Release \
    -o /app/build \
    --no-restore \
    --verbosity minimal

# Publish stage
FROM build AS publish
RUN dotnet publish "residence.api/residence.api.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false \
    --no-build \
    --verbosity minimal

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install curl for health checks and timezone data
RUN apt-get update && \
    apt-get install -y --no-install-recommends curl tzdata && \
    rm -rf /var/lib/apt/lists/* && \
    ln -sf /usr/share/zoneinfo/UTC /etc/localtime

# Copy published application
COPY --from=publish /app/publish .

# Create non-root user for security (optional but recommended)
RUN useradd -m -u 1000 appuser && chown -R appuser:appuser /app
USER appuser

# Expose ports
EXPOSE 8080
EXPOSE 8443

# Health check - verify API is responding
HEALTHCHECK --interval=30s --timeout=5s --start-period=60s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_EnableDiagnostics=0

# Start application
ENTRYPOINT ["dotnet", "residence.api.dll"]
