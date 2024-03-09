# https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/alpine3.19/amd64/Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0.201-alpine3.19-amd64 AS build
ARG C=Release
WORKDIR /src
COPY ["BlazorClient/", "BlazorClient/"]
COPY ["BlazorClient.Application/", "BlazorClient.Application/"]
COPY ["ProxyServer/", "ProxyServer/"]
COPY ["Shared/", "Shared/"]
COPY ["Directory.Build.props", "./"]
RUN dotnet restore ./ProxyServer/ProxyServer.csproj -r linux-musl-x64
RUN dotnet publish ./ProxyServer/ProxyServer.csproj -c ${C} -o /app/publish -r linux-musl-x64
RUN dotnet restore ./BlazorClient/BlazorClient.csproj
RUN dotnet publish ./BlazorClient/BlazorClient.csproj -c ${C} -o /app/publish


# https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.19/amd64/Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0.2-alpine3.19-amd64 AS final
ARG E=Production
ENV DOTNET_ENVIRONMENT=${E}
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish ./
ENTRYPOINT ./ProxyServer
