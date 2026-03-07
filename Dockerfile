# Etapa de build: compila tu proyecto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos primero el archivo .csproj para aprovechar la cache de Docker
COPY ["AuthService/AuthService.csproj", "AuthService/"]
WORKDIR /src/AuthService
RUN dotnet restore "AuthService.csproj"

# Copiamos el resto del código
WORKDIR /src
COPY . .

# Publicamos en modo Release
WORKDIR /src/AuthService
RUN dotnet publish -c Release -o /app

# Etapa de runtime: solo lo necesario para correr
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiamos la salida publicada
COPY --from=build /app ./

# Copiamos el archivo de configuración de log4net
COPY AuthService/log4net.config ./log4net.config

# Crear carpeta App_Data y copiar la base de datos GeoLite2
RUN mkdir -p /app/App_Data
COPY AuthService/App_Data/GeoLite2-City.mmdb /app/App_Data/GeoLite2-City.mmdb

# Crear carpeta Logs para asegurar que exista
RUN mkdir -p /app/Logs

ENTRYPOINT ["dotnet", "AuthService.dll"]