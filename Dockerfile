# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copier le csproj pour restore (cache optimisation)
COPY ["src/residence.api/residence.api.csproj", "residence.api/"]
COPY ["src/residence.application/residence.application.csproj", "residence.application/"]
COPY ["src/residence.infrastructure/residence.infrastructure.csproj", "residence.infrastructure/"]
COPY ["src/residence.domain/residence.domain.csproj", "residence.domain/"]

RUN dotnet restore "residence.api/residence.api.csproj"

# Copier le reste du code
COPY . .

WORKDIR "/src/WebApi"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false


# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# Port exposé (important pour cloud)
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "residence.api.dll"]