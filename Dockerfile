FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["YuGames.Application/YuGames.Application.csproj", "YuGames.Application/"]
COPY ["YuGames.Common/YuGames.Common.csproj", "YuGames.Common/"]
COPY ["YuGames.Domain/YuGames.Domain.csproj", "YuGames.Domain/"]
COPY ["YuGames.External/YuGames.External.csproj", "YuGames.External/"]
COPY ["YuGames.Persistence/YuGames.Persistence.csproj", "YuGames.Persistence/"]
COPY ["YuGames.Presentation/YuGames.Presentation.csproj", "YuGames.Presentation/"]

RUN dotnet restore "YuGames.Presentation/YuGames.Presentation.csproj"
COPY . .
WORKDIR "/src/YuGames.Presentation"
RUN dotnet build "YuGames.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YuGames.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YuGames.Presentation.dll"]