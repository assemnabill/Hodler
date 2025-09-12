# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /src

# Copy project files
COPY ["hodler-api/hodler-api.csproj", "hodler-api/"]
COPY ["Hodler.ApiService/Hodler.ApiService.csproj", "Hodler.ApiService/"]
RUN dotnet restore "hodler-api/hodler-api.csproj"

# Copy everything else
COPY . .

# Build and publish
WORKDIR "/src/hodler-api"
RUN dotnet publish "hodler-api.csproj" -c Release -o /app/publish \
    -r linux-arm64 \
    --self-contained false \
    --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-chiseled AS runtime
WORKDIR /app

# Install required dependencies for ARM64
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    libssl-dev \
    && rm -rf /var/lib/apt/lists/*

# Create a non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser:appuser /app
USER appuser

COPY --from=build --chown=appuser:appuser /app/publish .
EXPOSE 3000

ENTRYPOINT ["./hodler-api"]