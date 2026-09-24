FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar archivos de proyecto primero para cachear la restauración
COPY Usuarios.Api/Usuarios.Api.csproj Usuarios.Api/
COPY Usuarios.Application/Usuarios.Application.csproj Usuarios.Application/
COPY Usuarios.Domain/Usuarios.Domain.csproj Usuarios.Domain/
COPY Usuarios.Infrastructure/Usuarios.Infrastructure.csproj Usuarios.Infrastructure/
RUN dotnet restore Usuarios.Api/Usuarios.Api.csproj

# Copiar todo el código fuente
COPY . .

# Build y publish
WORKDIR /src/Usuarios.Api
RUN dotnet publish -c Release -o /app/publish --no-restore

# ── Etapa de runtime ──
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Usuarios.Api.dll"]
