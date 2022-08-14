FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_18.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["WannaEat.Web.csproj", "."]
RUN dotnet restore "WannaEat.Web.csproj"
COPY . .
WORKDIR /src
RUN dotnet build "WannaEat.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WannaEat.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app/publish
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WannaEat.Web.dll"]
