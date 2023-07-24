FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BotManager/BotManager.csproj", "BotManager/"]
RUN dotnet restore "BotManager/BotManager.csproj"
COPY . .
WORKDIR "/src/BotManager"
RUN dotnet build "BotManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BotManager.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BotManager.dll"]
