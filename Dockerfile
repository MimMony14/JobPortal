FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY JobPortal.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish JobPortal.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://0.0.0.0:8080

COPY --from=build /app/publish ./

EXPOSE 8080
ENTRYPOINT ["dotnet", "JobPortal.dll"]
