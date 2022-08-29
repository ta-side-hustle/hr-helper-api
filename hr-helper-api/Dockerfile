FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["hr-helper-api/hr-helper-api.csproj", "hr-helper-api/"]
RUN dotnet restore "hr-helper-api/hr-helper-api.csproj"
COPY . .
WORKDIR "/src/hr-helper-api"
RUN dotnet build "hr-helper-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "hr-helper-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "hr-helper-api.dll"]
