# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY --link ["./Gidbugds_BE.sln", "./"]
COPY --link ["./Gigbuds_BE.Domain/*.csproj", "Gigbuds_BE.Domain/"]
COPY --link ["./Gigbuds_BE.Infrastructure/*.csproj", "Gigbuds_BE.Infrastructure/"]
COPY --link ["./Gigbuds_BE.Application/*.csproj", "Gigbuds_BE.Application/"]
COPY --link ["./Gigbuds_BE.API/*.csproj", "Gigbuds_BE.API/"]
COPY --link ["./Gigbuds_BE.API/appsettings.json", "Gigbuds_BE.API/"]

RUN dotnet restore "./Gigbuds_BE.API/Gigbuds_BE.API.csproj"
COPY . .
WORKDIR "/src/Gigbuds_BE.API"
RUN dotnet build "./Gigbuds_BE.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Gigbuds_BE.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gigbuds_BE.API.dll"]