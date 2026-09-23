FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Admin.Api/Admin.Api.csproj Admin.Api/
COPY Admin.Application/Admin.Application.csproj Admin.Application/
COPY Admin.Domain/Admin.Domain.csproj Admin.Domain/
COPY Admin.Infrastructure/Admin.Infrastructure.csproj Admin.Infrastructure/
RUN dotnet restore Admin.Api/Admin.Api.csproj

COPY . .

WORKDIR /src/Admin.Api
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Admin.Api.dll"]
