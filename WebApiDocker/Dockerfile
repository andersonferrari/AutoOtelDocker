# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#TODO: check if we can run with less priviledges
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
# OpenTelemetry added in the base stage so it will work with VS debug
# TODO: check if it is needed: ENV OTEL_DOTNET_AUTO_HOME=/otel-ai
#RUN apt-get update && apt-get install -y curl unzip
#RUN curl -L -O https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation/releases/latest/download/otel-dotnet-auto-install.sh
#RUN ls
#RUN chmod +x otel-dotnet-auto-install.sh
#RUN ./otel-dotnet-auto-install.sh
#ENV TEST_VAR test
# TODO: do we need to install unzip in this container?
# TODO: check if it works
# chmod +x $OTEL_DOTNET_AUTO_HOME/instrument.sh
# $OTEL_DOTNET_AUTO_HOME/instrument.sh dotnet run
#RUN ./instrument.sh 


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebApiDocker/WebApiDocker.csproj", "WebApiDocker/"]
RUN dotnet restore "./WebApiDocker/WebApiDocker.csproj"
COPY . .
WORKDIR "/src/WebApiDocker"
RUN dotnet build "./WebApiDocker.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApiDocker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApiDocker.dll"]