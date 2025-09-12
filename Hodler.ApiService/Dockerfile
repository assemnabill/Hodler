FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY Hodler.ApiService/Hodler.ApiService.csproj Hodler.ApiService/
RUN dotnet restore "Hodler.ApiService/Hodler.ApiService.csproj"
COPY . .
RUN dotnet publish "Hodler.ApiService/Hodler.ApiService.csproj" -c Release -o /app/publish

FROM --platform=$TARGETPLATFORM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
# Create a non-root user and group
RUN useradd --no-log-init --system --user-group --home-dir /app appuser
RUN chown -R appuser:appuser /app
USER appuser
ENTRYPOINT ["dotnet", "Hodler.ApiService.dll"]
