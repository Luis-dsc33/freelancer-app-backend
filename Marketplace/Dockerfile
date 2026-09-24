FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Marketplace.Api/Marketplace.Api.csproj Marketplace.Api/
COPY Marketplace.Application/Marketplace.Application.csproj Marketplace.Application/
COPY Marketplace.Domain/Marketplace.Domain.csproj Marketplace.Domain/
COPY Marketplace.Infrastructure/Marketplace.Infrastructure.csproj Marketplace.Infrastructure/
RUN dotnet restore Marketplace.Api/Marketplace.Api.csproj

COPY . .

WORKDIR /src/Marketplace.Api
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Marketplace.Api.dll"]
