FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["GameOn.Application/GameOn.Application.csproj", "GameOn.Application/"]
COPY ["GameOn.Common/GameOn.Common.csproj", "GameOn.Common/"]
COPY ["GameOn.Domain/GameOn.Domain.csproj", "GameOn.Domain/"]
COPY ["GameOn.External/GameOn.External.csproj", "GameOn.External/"]
COPY ["GameOn.Persistence/GameOn.Persistence.csproj", "GameOn.Persistence/"]
COPY ["GameOn.Presentation/GameOn.Presentation.csproj", "GameOn.Presentation/"]

RUN dotnet restore "GameOn.Presentation/GameOn.Presentation.csproj"
COPY . .
WORKDIR "/src/GameOn.Presentation"
RUN dotnet build "GameOn.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameOn.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameOn.Presentation.dll"]