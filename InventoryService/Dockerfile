FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY InventoryService/*.csproj ./InventoryService/
RUN dotnet restore InventoryService/InventoryService.csproj

COPY InventoryService/. ./InventoryService/
WORKDIR /app/InventoryService
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/InventoryService/out ./
EXPOSE 5071
ENTRYPOINT ["dotnet", "InventoryService.dll"]
