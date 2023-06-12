# Build container. Load full SDK as base image.
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY . ./
RUN dotnet restore POS.sln

WORKDIR /src/DocumentManager.Webapi
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /src
COPY --from=build /src/DocumentManager.Webapi/out .
ENTRYPOINT ["dotnet", "DocumentManager.Webapi.dll"]