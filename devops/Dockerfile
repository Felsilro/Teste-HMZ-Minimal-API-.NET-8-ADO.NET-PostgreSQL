FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ./src/HMZ.Domain/HMZ.Domain.csproj ./HMZ.Domain/
COPY ./src/HMZ.Application/HMZ.Application.csproj ./HMZ.Application/
COPY ./src/HMZ.Api/HMZ.Api.csproj ./HMZ.Api/
RUN dotnet restore ./HMZ.Api/HMZ.Api.csproj

COPY ./src ./
RUN dotnet publish ./HMZ.Api/HMZ.Api.csproj -c Release -o /out

FROM base AS final
WORKDIR /app
COPY --from=build /out ./
COPY ./.env ./
ENTRYPOINT ["dotnet", "HMZ.Api.dll"]
