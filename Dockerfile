#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM node:12 AS nodeBuild
WORKDIR /app
COPY ./SPA/ /app
RUN npm install
RUN npm build
 
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
 
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
 
COPY . .
WORKDIR /src/nibss_orchad_azure
 
RUN dotnet publish "nibss_orchad_azure.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
COPY --from=build /src/nibss_orchad_azure /app
COPY --from=nodeBuild /app/dist/js /app/wwwroot/js
 
ENTRYPOINT ["dotnet", "nibss_orchad_azure.dll","--urls", "http://0.0.0.0:80"]