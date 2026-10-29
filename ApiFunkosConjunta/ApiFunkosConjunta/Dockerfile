# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Directory.Packages.props ./
COPY Directory.Build.props ./

COPY ApiFunkosConjunta/*.csproj ./ApiFunkosConjunta/

RUN dotnet restore ApiFunkosConjunta/ApiFunkosConjunta.csproj

COPY ApiFunkosConjunta/ ./ApiFunkosConjunta/

RUN dotnet publish ApiFunkosConjunta/ApiFunkosConjunta.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

# Directorio de logs escribible por el usuario de la aplicacion
RUN mkdir -p /app/logs && chown -R app:app /app/logs

USER app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "ApiFunkosConjunta.dll"]
