# Etapa 1: Compilación (SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos primero el archivo del proyecto y restauramos dependencias (aprovechando caché de capas de Docker)
COPY ["Restaurante.csproj", "./"]
RUN dotnet restore "Restaurante.csproj"

# Copiamos todo el resto del código
COPY . .

# Compilamos la aplicación en modo Release
RUN dotnet publish "Restaurante.csproj" -c Release -o /app/publish

# Etapa 2: Runtime (ASP.NET Core)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copiamos la aplicación compilada de la etapa anterior
COPY --from=build /app/publish .

# En .NET 8, el puerto por defecto en los contenedores es el 8080
EXPOSE 8080

# Definimos el comando de inicio
ENTRYPOINT ["dotnet", "Restaurante.dll"]
