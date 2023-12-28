FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG WATCHIT_API_NUGET_USERNAME
ARG WATCHIT_API_NUGET_PASSWORD
WORKDIR /src
COPY . /src
RUN dotnet nuget add source https://nuget.pkg.github.com/WatchITProject/index.json --username $WATCHIT_API_NUGET_USERNAME --password $WATCHIT_API_NUGET_PASSWORD --store-password-in-clear-text
RUN dotnet restore "WatchIT.WebAPI.sln"
RUN dotnet build -c Release -o /app/build "WatchIT.WebAPI.sln"
RUN dotnet dev-certs https --trust
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false "WatchIT.WebAPI.sln"


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS run
WORKDIR /app
COPY --from=build /root/.dotnet/corefx/cryptography/x509stores/my/* /root/.dotnet/corefx/cryptography/x509stores/my/
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=https://+:443;http://+:80
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "WatchIT.WebAPI.dll"]
