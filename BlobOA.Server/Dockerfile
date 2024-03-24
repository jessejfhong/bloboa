# https://github.com/dotnet/dotnet-docker/blob/main/src/sdk/8.0/alpine3.19/amd64/Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0.203-bookworm-slim-amd64 AS build-stage1
ARG C=Release
WORKDIR /src
COPY ["BlobOA.Application/", "BlobOA.Application/"]
COPY ["BlobOA.Infrastructure/", "BlobOA.Infrastructure/"]
COPY ["BlobOA.Server/", "BlobOA.Server/"]
COPY ["BlobOA.Shared.Dtos/", "BlobOA.Shared.Dtos/"]
COPY ["BlobOA.Shared.Messages/", "BlobOA.Shared.Messages/"]
COPY ["Directory.Build.props", "./"]
RUN dotnet restore ./BlobOA.Server/BlobOA.Server.csproj -r linux-musl-x64
RUN dotnet publish ./BlobOA.Server/BlobOA.Server.csproj -c ${C} -o /app/publish -r linux-musl-x64 --no-restore


FROM build-stage1 AS build-stage2
ARG C=Release
COPY ["BlobOA.BlazorClient/", "BlobOA.BlazorClient/"]
RUN dotnet restore ./BlobOA.BlazorClient/BlobOA.BlazorClient.csproj
RUN dotnet publish ./BlobOA.BlazorClient/BlobOA.BlazorClient.csproj -c ${C} -o /app/publish --no-restore


# https://github.com/dotnet/dotnet-docker/blob/main/src/aspnet/8.0/alpine3.19/amd64/Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0.2-alpine3.19-amd64 AS final-base
ARG E=Production
ENV DOTNET_ENVIRONMENT=${E}
WORKDIR /app
EXPOSE 8080
ENTRYPOINT ./BlobOA-Server


FROM final-base AS final-api
COPY --from=build-stage1 /app/publish ./

FROM final-base AS final-full
COPY --from=build-stage2 /app/publish ./
