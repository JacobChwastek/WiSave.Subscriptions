FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WiSave.Subscriptions.WebJob.Domain/WiSave.Subscriptions.WebJob.Domain.csproj", "src/WiSave.Subscriptions.WebJob.Domain/"]
RUN dotnet restore "src/WiSave.Subscriptions.WebJob.Domain/WiSave.Subscriptions.WebJob.Domain.csproj"
COPY . .
WORKDIR "/src/src/WiSave.Subscriptions.WebJob.Domain"
RUN dotnet build "./WiSave.Subscriptions.WebJob.Domain.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WiSave.Subscriptions.WebJob.Domain.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WiSave.Subscriptions.WebJob.Domain.dll"]
