# Etapa de build: compila tu proyecto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore              # Descarga dependencias
RUN dotnet publish -c Release -o /app   # Compila y publica en modo Release

# Etapa de runtime: solo lo necesario para correr
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .        # Copia los binarios compilados
ENTRYPOINT ["dotnet", "AuthService.dll"]  # Arranca tu API