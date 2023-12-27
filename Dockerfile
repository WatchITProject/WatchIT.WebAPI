FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY [".", "/src"]
RUN dotnet nuget add source https://nuget.pkg.github.com/WatchITProject/index.json --username ${WATCHIT_API_NUGET_USERNAME} --password ${WATCHIT_API_NUGET_PASSWORD} --store-password-in-clear-text
RUN dotnet restore "WatchIT.WebAPI.sln"
RUN dotnet build --no-restore -c Release -o /app/build "WatchIT.WebAPI.sln"

FROM build AS publish
WORKDIR /src
RUN dotnet publish "WatchIT.WebAPI.sln" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WatchIT.WebAPI.dll"]