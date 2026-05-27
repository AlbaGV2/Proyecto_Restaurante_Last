FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# copy csproj and restore as distinct layers
COPY ["Restaurante.csproj", "./"]
RUN dotnet restore "Restaurante.csproj"
# copy everything else and build
COPY . .
RUN dotnet publish "Restaurante.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
USER app
ENTRYPOINT ["dotnet", "Restaurante.dll"]
