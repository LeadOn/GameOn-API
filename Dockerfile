FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

#ENV ASPNETCORE_URLS="https://+;http://+"
#ENV ASPNETCORE_Kestrel__Certificates__Default__Path=certificate.pfx
#ENV ASPNETCORE_Kestrel__Certificates__Default__Password="test"

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["YuFoot.Business/YuFoot.Business.csproj", "YuFoot.Business/"]
COPY ["YuFoot.Business.Contracts/YuFoot.Business.Contracts.csproj", "YuFoot.Business.Contracts/"]
COPY ["YuFoot.DTOs/YuFoot.DTOs.csproj", "YuFoot.DTOs/"]
COPY ["YuFoot.Entities/YuFoot.Entities.csproj", "YuFoot.Entities/"]
COPY ["YuFoot.Repository/YuFoot.Repository.csproj", "YuFoot.Repository/"]
COPY ["YuFoot.Repository.Contracts/YuFoot.Repository.Contracts.csproj", "YuFoot.Repository.Contracts/"]
COPY ["YuFoot.WebAPI/YuFoot.WebAPI.csproj", "YuFoot.WebAPI/"]

RUN dotnet restore "YuFoot.WebAPI/YuFoot.WebAPI.csproj"
COPY . .
WORKDIR "/src/YuFoot.WebAPI"
RUN dotnet build "YuFoot.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YuFoot.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
#COPY ["FrontFactory.Parser.Presentation/certificate.pfx", "."]
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YuFoot.WebAPI.dll"]