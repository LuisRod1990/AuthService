# Etapa 1: Build y pruebas
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar csproj y restaurar dependencias
COPY AuthService/*.csproj AuthService/
COPY AuthService.Test/*.csproj AuthService.Test/
RUN dotnet restore AuthService/AuthService.csproj

# Copiar todo el código
COPY . .

# Ejecutar pruebas (xUnit)
RUN dotnet test AuthService.Test/AuthService.Test.csproj --logger "trx;LogFileName=test_results.trx"

# Publicar la API
RUN dotnet publish AuthService/AuthService.csproj -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar artefactos publicados
COPY --from=build /app/publish .

# Cloud Run usa el puerto 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "AuthService.dll"]

