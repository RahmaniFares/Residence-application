# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["residence.api/residence.api.csproj", "residence.api/"]
COPY ["residence.application/residence.application.csproj", "residence.application/"]
COPY ["residence.infrastructure/residence.infrastructure.csproj", "residence.infrastructure/"]
COPY ["residence.domain/residence.domain.csproj", "residence.domain/"]

# Restore dependencies
RUN dotnet restore "residence.api/residence.api.csproj"

# Copy source code
COPY . .

# Build application
WORKDIR /src/residence.api
RUN dotnet build "residence.api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "residence.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install timezone data (optional but useful)
RUN apt-get update && apt-get install -y tzdata && rm -rf /var/lib/apt/lists/*

# Copy published application
COPY --from=publish /app/publish .

# Create non-root user for security
RUN useradd -m -u 1000 appuser && chown -R appuser:appuser /app
USER appuser

# Expose port
EXPOSE 8080
EXPOSE 8443

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Start application
ENTRYPOINT ["dotnet", "residence.api.dll"]
