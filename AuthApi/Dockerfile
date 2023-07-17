#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
#WORKDIR /src
#COPY ["AuthApi/AuthApi.csproj", "AuthApi/"]
#RUN dotnet restore "AuthApi/AuthApi.csproj"
#COPY . .
#WORKDIR "/src/AuthApi"
#RUN dotnet build "AuthApi.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "AuthApi.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "AuthApi.dll"]
#

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /code
EXPOSE 80
EXPOSE 443

COPY . .

RUN dotnet restore

RUN dotnet publish -o /out/ -c Release

RUN dotnet dev-certs https --clean
RUN dotnet dev-certs https -ep ./AuthApi.pfx  -p crypticpassword
RUN mv ./AuthApi.pfx /out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base

COPY --from=build /out /app/
WORKDIR /app

ENTRYPOINT ["dotnet", "AuthApi.dll"]