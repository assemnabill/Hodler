FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY Hodler.AppHost/Hodler.AppHost.csproj Hodler.AppHost/
RUN dotnet restore "Hodler.AppHost/Hodler.AppHost.csproj"
COPY . .
RUN dotnet publish "Hodler.AppHost/Hodler.AppHost.csproj" -c Release -o /app/publish

FROM --platform=$TARGETPLATFORM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
# Create a non-root user and group
RUN useradd --no-log-init --system --user-group --home-dir /app appuser
RUN chown -R appuser:appuser /app
USER appuser
ENTRYPOINT ["dotnet", "Hodler.AppHost.dll"]
