#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
#WORKDIR /app
#EXPOSE 80
#
#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
#WORKDIR /src
#COPY ["WebMvc/WebMvc.csproj", "WebMvc/"]
#RUN dotnet restore "WebMvc/WebMvc.csproj"
#COPY . .
#WORKDIR "/src/WebMvc"
#RUN dotnet build "WebMvc.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "WebMvc.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "WebMvc.dll"]


FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /code
EXPOSE 80
EXPOSE 443

COPY . .

RUN dotnet restore

RUN dotnet publish -o /out/ -c Release

RUN dotnet dev-certs https --clean
RUN dotnet dev-certs https -ep ./WebMvc.pfx  -p crypticpassword
RUN mv ./WebMvc.pfx /out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base

COPY --from=build /out /app/

WORKDIR /app

ENTRYPOINT ["dotnet", "WebMvc.dll"]