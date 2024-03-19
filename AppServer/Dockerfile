# https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/alpine3.19/amd64/Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0.201-alpine3.19-amd64 AS build-stage1
ARG C=Release
WORKDIR /src
COPY ["AppServer/", "AppServer/"]
COPY ["BlobOA.Shared.Dtos/", "BlobOA.Shared.Dtos/"]
COPY ["BlobOA.Shared.Messages/", "BlobOA.Shared.Messages/"]
COPY ["Directory.Build.props", "./"]
RUN dotnet restore ./AppServer/AppServer.csproj -r linux-musl-x64
RUN dotnet publish ./AppServer/AppServer.csproj -c ${C} -o /app/publish -r linux-musl-x64


FROM build-stage1 AS build-stage2
ARG C=Release
COPY ["BlazorClient/", "BlazorClient/"]
COPY ["BlazorClient.Application/", "BlazorClient.Application/"]
RUN dotnet restore ./BlazorClient/BlazorClient.csproj
RUN dotnet publish ./BlazorClient/BlazorClient.csproj -c ${C} -o /app/publish


# https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.19/amd64/Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0.2-alpine3.19-amd64 AS final-base
ARG E=Production
ENV DOTNET_ENVIRONMENT=${E}
WORKDIR /app
EXPOSE 8080
ENTRYPOINT ./bloboa-server


FROM final-base AS final-api
COPY --from=build-stage1 /app/publish ./

FROM final-base AS final-full
COPY --from=build-stage2 /app/publish ./
