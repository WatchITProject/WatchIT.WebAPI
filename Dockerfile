FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG WATCHIT_API_NUGET_USERNAME
ARG WATCHIT_API_NUGET_PASSWORD
ARG WATCHIT_CERTIFICATE_PASSWORD
WORKDIR /src
COPY . /src
ENV ENABLE_CORS=true
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=$WATCHIT_CERTIFICATE_PASSWORD
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/cert/WatchIT.WebAPI.pfx
RUN dotnet nuget add source https://nuget.pkg.github.com/WatchITProject/index.json --username $WATCHIT_API_NUGET_USERNAME --password $WATCHIT_API_NUGET_PASSWORD --store-password-in-clear-text
RUN dotnet dev-certs https -ep /cert/WatchIT.WebAPI.pfx -p $WATCHIT_CERTIFICATE_PASSWORD
RUN dotnet dev-certs https --trust
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false "WatchIT.WebAPI.sln"

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS run
ARG WATCHIT_CERTIFICATE_PASSWORD
WORKDIR /app
COPY --from=build /root/.dotnet/corefx/cryptography/x509stores/my/* /root/.dotnet/corefx/cryptography/x509stores/my/
COPY --from=build /app/publish .
COPY --from=build /cert/* /cert/
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=https://+:443;http://+:80
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=$WATCHIT_CERTIFICATE_PASSWORD
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/cert/WatchIT.WebAPI.pfx
ENV ENABLE_CORS=true
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "WatchIT.WebAPI.dll"]
