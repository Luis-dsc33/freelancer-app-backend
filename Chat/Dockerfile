FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Chat.Api/Chat.Api.csproj Chat.Api/
COPY Chat.Application/Chat.Application.csproj Chat.Application/
COPY Chat.Domain/Chat.Domain.csproj Chat.Domain/
COPY Chat.Infrastructure/Chat.Infrastructure.csproj Chat.Infrastructure/
RUN dotnet restore Chat.Api/Chat.Api.csproj

COPY . .

WORKDIR /src/Chat.Api
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Chat.Api.dll"]
