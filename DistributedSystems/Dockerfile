FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DistributedSystems/DistributedSystems.csproj", "DistributedSystems/"]
RUN dotnet restore "DistributedSystems/DistributedSystems.csproj"
COPY . .
WORKDIR "/src/DistributedSystems"
RUN dotnet build "DistributedSystems.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DistributedSystems.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DistributedSystems.dll"]
