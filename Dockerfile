# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia tudo
COPY . ./

# Restaura dependências
RUN dotnet restore

# Publica em modo Release
RUN dotnet publish -c Release -o /out

# Etapa final (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .

# Expõe a porta 80
EXPOSE 80

ENTRYPOINT ["dotnet", "MyStoreApi.dll"]
