# Build context must be the repository root (e.g. DockerfileContext=..\..).
# For private NuGet feed: docker build --build-arg PersonalAccessToken=... .
# See https://aka.ms/containerfastmode for Visual Studio container tooling.

FROM mcr.microsoft.com/dotnet/aspnet:9.0.0-noble-amd64 AS base
WORKDIR /app
EXPOSE 14602

FROM mcr.microsoft.com/dotnet/sdk:9.0.100-noble-amd64 AS build
ARG PersonalAccessToken
WORKDIR /src

# Root build and NuGet config
COPY Directory.Build.* ./
COPY NuGet.config .
COPY global.json ./

# Project files only (order supports layer caching; restore needs all referenced projects)
COPY ["src/GtMotive.Estimate.Microservice.Host/GtMotive.Estimate.Microservice.Host.csproj", "GtMotive.Estimate.Microservice.Host/"]
COPY ["src/GtMotive.Estimate.Microservice.Api/GtMotive.Estimate.Microservice.Api.csproj", "GtMotive.Estimate.Microservice.Api/"]
COPY ["src/GtMotive.Estimate.Microservice.ApplicationCore/GtMotive.Estimate.Microservice.ApplicationCore.csproj", "GtMotive.Estimate.Microservice.ApplicationCore/"]
COPY ["src/GtMotive.Estimate.Microservice.Infrastructure/GtMotive.Estimate.Microservice.Infrastructure.csproj", "GtMotive.Estimate.Microservice.Infrastructure/"]
COPY ["src/GtMotive.Estimate.Microservice.Domain/GtMotive.Estimate.Microservice.Domain.csproj", "GtMotive.Estimate.Microservice.Domain/"]

# Credential provider for private feed. Pass token with: --build-arg PersonalAccessToken=...
RUN wget -qO- https://raw.githubusercontent.com/Microsoft/artifacts-credprovider/master/helpers/installcredprovider.sh | bash
ENV NUGET_CREDENTIALPROVIDER_SESSIONTOKENCACHE_ENABLED=true
ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS="{\"endpointCredentials\": [{\"endpoint\":\"https://pkgs.dev.azure.com/gtmotive/_packaging/GtMotivePackages/nuget/v3/index.json\", \"password\":\"${PersonalAccessToken}\"}]}"
RUN dotnet restore "GtMotive.Estimate.Microservice.Host/GtMotive.Estimate.Microservice.Host.csproj"

# Full source for build (no test or other repo root content)
COPY src .
COPY .editorconfig ./

WORKDIR "/src/GtMotive.Estimate.Microservice.Host"
RUN dotnet build "GtMotive.Estimate.Microservice.Host.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/GtMotive.Estimate.Microservice.Host"
RUN dotnet publish "GtMotive.Estimate.Microservice.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT="Development"
ENV ASPNETCORE_URLS="http://+:14602"
# Default for standalone run (e.g. docker run). Use host.docker.internal to reach Mongo on host (e.g. mongodb-vs with -p 27017:27017). Overridden by docker-compose to mongodb:27017.
ENV MongoDb__ConnectionString="mongodb://admin:admin@host.docker.internal:27017"
ENV AppSettings__LocalAuthTokenUrl="http://localhost:14602/connect/token"
ENTRYPOINT ["dotnet", "GtMotive.Estimate.Microservice.Host.dll"]
