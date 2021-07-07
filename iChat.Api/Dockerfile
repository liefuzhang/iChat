#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
RUN curl -sL https://deb.nodesource.com/setup_14.x |  bash -
RUN apt-get install -y nodejs

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
RUN curl -sL https://deb.nodesource.com/setup_14.x |  bash -
RUN apt-get install -y nodejs
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
ENTRYPOINT ["dotnet", "iChat.Api.dll"]