# Build stage
FROM mcr.microsoft.com/dotnet/aspnet:9.4 AS build
WORKDIR /src

# Copy project files
COPY ["Hodler.ApiService/Hodler.ApiService.csproj", "Hodler.ApiService/"]
RUN dotnet restore "Hodler.ApiService\Hodler.ApiService.csproj"

# Copy everything else
COPY . .

# Build and publish
WORKDIR "/src/Hodler.ApiService"
RUN dotnet publish "Hodler.ApiService.csproj" -c Release -o /app/publish \
    -r linux-arm64 \
    --self-contained false \
    --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.4 AS runtime
WORKDIR /app
COPY ./publish .
ENTRYPOINT ["dotnet", "Hodler.ApiService.dll"]
