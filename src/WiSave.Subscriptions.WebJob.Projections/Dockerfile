﻿FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WiSave.Subscriptions.WebJob.Projections/WiSave.Subscriptions.WebJob.Projections.csproj", "src/WiSave.Subscriptions.WebJob.Projections/"]
RUN dotnet restore "src/WiSave.Subscriptions.WebJob.Projections/WiSave.Subscriptions.WebJob.Projections.csproj"
COPY . .
WORKDIR "/src/src/WiSave.Subscriptions.WebJob.Projections"
RUN dotnet build "./WiSave.Subscriptions.WebJob.Projections.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WiSave.Subscriptions.WebJob.Projections.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WiSave.Subscriptions.WebJob.Projections.dll"]
