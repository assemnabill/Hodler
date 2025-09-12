FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY Hodler.ApiService/Hodler.ApiService.csproj Hodler.ApiService/
RUN dotnet restore "Hodler.ApiService/Hodler.ApiService.csproj"
COPY . .
RUN dotnet publish "Hodler.ApiService/Hodler.ApiService.csproj" -c Release -o /app/publish

FROM --platform=$TARGETPLATFORM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Hodler.ApiService.dll"]
