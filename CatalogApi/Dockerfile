#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
#WORKDIR /app
#EXPOSE 80
#
#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
#WORKDIR /src
#COPY ["CatalogApi.csproj", "CatalogApi/"]
#RUN dotnet restore "./CatalogApi/CatalogApi.csproj"
#COPY . .
#WORKDIR ./src
#RUN dotnet build "./CatalogApi/CatalogApi.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "CatalogApi.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "CatalogApi.dll"]

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /code
EXPOSE 80

COPY . .

RUN dotnet restore

RUN dotnet publish -o /out/ -c Release

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base

COPY --from=build /out /app/
#COPY ./wwwroot ./wwwroot  # Add this line to copy wwwroot folder
WORKDIR /app

ENTRYPOINT ["dotnet", "CatalogApi.dll"]