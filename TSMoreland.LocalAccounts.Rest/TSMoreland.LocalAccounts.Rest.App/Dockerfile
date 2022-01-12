#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TSMoreland.LocalAccounts.Rest.App/TSMoreland.LocalAccounts.Rest.App.csproj", "TSMoreland.LocalAccounts.Rest.App/"]
RUN dotnet restore "TSMoreland.LocalAccounts.Rest.App/TSMoreland.LocalAccounts.Rest.App.csproj"
COPY . .
WORKDIR "/src/TSMoreland.LocalAccounts.Rest.App"
RUN dotnet build "TSMoreland.LocalAccounts.Rest.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TSMoreland.LocalAccounts.Rest.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TSMoreland.LocalAccounts.Rest.App.dll"]