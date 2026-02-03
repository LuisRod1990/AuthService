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
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "AuthService.dll"]