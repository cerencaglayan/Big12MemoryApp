# =========================
# Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# =========================
# Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Big12MemoryApp.API/Big12MemoryApp.API.csproj", "Big12MemoryApp.API/"]
COPY ["Big12MemoryApp.Application/Big12MemoryApp.Application.csproj", "Big12MemoryApp.Application/"]
COPY ["Big12MemoryApp.Domain/Big12MemoryApp.Domain.csproj", "Big12MemoryApp.Domain/"]
COPY ["Big12MemoryApp.Infrastructure/Big12MemoryApp.Infrastructure.csproj", "Big12MemoryApp.Infrastructure/"]

RUN dotnet restore "Big12MemoryApp.API/Big12MemoryApp.API.csproj"

COPY . .
WORKDIR "/src/Big12MemoryApp.API"
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# =========================
# Final
# =========================
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Big12MemoryApp.API.dll"]
