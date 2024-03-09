# https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/alpine3.19/amd64/Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0.201-alpine3.19-amd64 AS build
ARG C=Release
WORKDIR /src
COPY ["EmailServer/", "EmailServer/"]
COPY ["Directory.Build.props", "./"]
RUN dotnet restore ./EmailServer/EmailServer.csproj -r linux-musl-x64
RUN dotnet publish ./EmailServer/EmailServer.csproj -c ${C} -o /app/publish -r linux-musl-x64


# https://github.com/dotnet/dotnet-docker/blob/main/src/runtime/8.0/alpine3.19/amd64/Dockerfile
FROM mcr.microsoft.com/dotnet/runtime:8.0.2-alpine3.19-amd64 AS final
ARG E=Production
ENV DOTNET_ENVIRONMENT=${E}
WORKDIR /app
COPY --from=build /app/publish ./
ENTRYPOINT ./EmailServer

