# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /src

# Copy project files
COPY ["Hodler.ApiService/Hodler.ApiService.csproj", "Hodler.ApiService/"]
RUN dotnet restore .\Hodler.ApiService\Hodler.ApiService.csproj

# Copy everything else
COPY . .

# Build and publish
WORKDIR "/src/Hodler.ApiService"
RUN dotnet publish "Hodler.ApiService.csproj" -c Release -o /app/publish \
    -r linux-arm64 \
    --self-contained false \
    --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-chiseled AS runtime
WORKDIR /app

# Create a non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser:appuser /app
USER appuser

COPY --from=build --chown=appuser:appuser /app/publish .
EXPOSE 3000

ENTRYPOINT ["./Hodler.ApiService"]