FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_18.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src

COPY ["src/WannaEat.Domain", "WannaEat.Domain"]
COPY ["src/WannaEat.Shared", "WannaEat.Shared"]
COPY ["src/WannaEat.Infrastructure", "WannaEat.Infrastructure"]
COPY ["src/WannaEat.FoodService.MMenu", "WannaEat.FoodService.MMenu"]
COPY ["src/WannaEat.FoodService.MZR", "WannaEat.FoodService.MZR"]
COPY ["src/WannaEat.Web", "WannaEat.Web"]
COPY ["src/WannaEat.sln", "."]
RUN dotnet restore WannaEat.sln

WORKDIR /src
RUN dotnet build WannaEat.Web/WannaEat.Web.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WannaEat.Web/WannaEat.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app/publish
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WannaEat.Web.dll"]
