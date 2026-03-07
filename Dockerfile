# Etapa de build: compila tu proyecto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["AuthService/AuthService.csproj", "AuthService/"]
WORKDIR /src/AuthService
RUN dotnet restore "AuthService.csproj"

WORKDIR /src
COPY . .

WORKDIR /src/AuthService
RUN dotnet publish -c Release -o /app

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app ./
COPY AuthService/log4net.config ./log4net.config

RUN mkdir -p /app/App_Data
COPY AuthService/App_Data/GeoLite2-City.mmdb /app/App_Data/GeoLite2-City.mmdb

RUN mkdir -p /app/Logs

ENTRYPOINT ["dotnet", "AuthService.dll"]