#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WebAppTinhVanCat_aspnetcore/WebAppTinhVanCat_aspnetcore.csproj", "WebAppTinhVanCat_aspnetcore/"]
RUN dotnet restore "WebAppTinhVanCat_aspnetcore/WebAppTinhVanCat_aspnetcore.csproj"
COPY . .
WORKDIR "/src/WebAppTinhVanCat_aspnetcore"
RUN dotnet build "WebAppTinhVanCat_aspnetcore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebAppTinhVanCat_aspnetcore.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAppTinhVanCat_aspnetcore.dll"]