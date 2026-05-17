# Build stage using SDK 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy UI project and restore dependencies
COPY ["Portfolio.UI/Portfolio.UI.csproj", "Portfolio.UI/"]
RUN dotnet restore "Portfolio.UI/Portfolio.UI.csproj"

# Copy all files and publish Blazor WASM
COPY . .
WORKDIR "/src/Portfolio.UI"
RUN dotnet publish "Portfolio.UI.csproj" -c Release -o /app/publish

# Nginx stage to serve static files
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
COPY Portfolio.UI/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
