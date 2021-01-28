# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0.102-ca-patch-buster-slim AS build
EXPOSE 80
WORKDIR /source

# copy csproj and restore as distinct layers
COPY src/Catalog/Catalog.API/Catalog.API.csproj .
RUN dotnet restore

# copy everything else and build app
COPY src/Catalog/Catalog.API/ .
RUN dotnet publish -c release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Catalog.API.dll"]