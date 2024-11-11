FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["client-credentials-flow/client-credentials-flow.csproj", "client-credentials-flow/"]
RUN dotnet restore "client-credentials-flow/client-credentials-flow.csproj"
COPY . .
WORKDIR "/src/client-credentials-flow"
RUN dotnet build "client-credentials-flow.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "client-credentials-flow.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "client-credentials-flow.dll"]
