#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TSMoreland.AspNetCore.RestartOnRequest.csproj", "."]
RUN dotnet restore "./TSMoreland.AspNetCore.RestartOnRequest.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TSMoreland.AspNetCore.RestartOnRequest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TSMoreland.AspNetCore.RestartOnRequest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TSMoreland.AspNetCore.RestartOnRequest.dll"]