FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Build the project
COPY . .
RUN dotnet restore
RUN dotnet build "BotManager/BotManager.csproj" -c Release -o /app/build

# Publish the project
FROM build AS publish
RUN dotnet publish "BotManager/BotManager.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Pack the standalone image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ./BotManager/run.json /config/run.json
ENTRYPOINT ["dotnet", "BotManager.dll", "/config/run.json"]