# Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["Portfolio.API/Portfolio.API.csproj", "Portfolio.API/"]
COPY ["Portfolio.Business/Portfolio.Business.csproj", "Portfolio.Business/"]
COPY ["Portfolio.DataAccess/Portfolio.DataAccess.csproj", "Portfolio.DataAccess/"]
COPY ["Portfolio.Domain/Portfolio.Domain.csproj", "Portfolio.Domain/"]
RUN dotnet restore "Portfolio.API/Portfolio.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/Portfolio.API"
RUN dotnet build "Portfolio.API.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "Portfolio.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Portfolio.API.dll"]
