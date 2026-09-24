FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Resenas.Api/Resenas.Api.csproj Resenas.Api/
COPY Resenas.Application/Resenas.Application.csproj Resenas.Application/
COPY Resenas.Domain/Resenas.Domain.csproj Resenas.Domain/
COPY Resenas.Infrastructure/Resenas.Infrastructure.csproj Resenas.Infrastructure/
RUN dotnet restore Resenas.Api/Resenas.Api.csproj

COPY . .

WORKDIR /src/Resenas.Api
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Resenas.Api.dll"]
