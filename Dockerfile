FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["YuGames.Business/YuGames.Business.csproj", "YuGames.Business/"]
COPY ["YuGames.Business.Contracts/YuGames.Business.Contracts.csproj", "YuGames.Business.Contracts/"]
COPY ["YuGames.DTOs/YuGames.DTOs.csproj", "YuGames.DTOs/"]
COPY ["YuGames.Common/YuGames.Common.csproj", "YuGames.Common/"]
COPY ["YuGames.Entities/YuGames.Entities.csproj", "YuGames.Entities/"]
COPY ["YuGames.Repository/YuGames.Repository.csproj", "YuGames.Repository/"]
COPY ["YuGames.Repository.Contracts/YuGames.Repository.Contracts.csproj", "YuGames.Repository.Contracts/"]
COPY ["YuGames.WebAPI/YuGames.WebAPI.csproj", "YuGames.WebAPI/"]

RUN dotnet restore "YuGames.WebAPI/YuGames.WebAPI.csproj"
COPY . .
WORKDIR "/src/YuGames.WebAPI"
RUN dotnet build "YuGames.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YuGames.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YuGames.WebAPI.dll"]