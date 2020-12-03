#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443

ENV TZ=Pacific/Auckland
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["iChat.Api/iChat.Api.csproj", "iChat.Api/"]
RUN dotnet restore "iChat.Api/iChat.Api.csproj"
COPY . .
WORKDIR "/src/iChat.Api"
RUN dotnet build "iChat.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "iChat.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet iChat.Api.dll