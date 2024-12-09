# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#TODO: check if we can run with less priviledges
#USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
# OpenTelemetry added in the base stage so it will work with VS debug
# TODO: check if it is needed: ENV OTEL_DOTNET_AUTO_HOME=/otel-ai
# download the open telemetry SDK script
ENV OTEL_DOTNET_AUTO_HOME /otel-ai
ENV TEST_VAR_STAGE1 loading2
RUN apt-get update && apt-get install -y curl unzip
WORKDIR /app
RUN curl -L -O https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation/releases/latest/download/otel-dotnet-auto-install.sh
# both commnand work... we can remove that: ADD https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation/releases/latest/download/otel-dotnet-auto-install.sh aTEST-otel-dotnet-auto-install.sh 
RUN chmod +x otel-dotnet-auto-install.sh
RUN ./otel-dotnet-auto-install.sh
# TODO: do we need to install unzip in this container?
# TODO: check if it works
RUN chmod +x $OTEL_DOTNET_AUTO_HOME/instrument.sh
#RUN ./instrument.sh 
RUN $OTEL_DOTNET_AUTO_HOME/instrument.sh 

# TODO: the vars are not going system wide, maybe create another script to re-export them system wide
# e.g.: RUN export-system-wide CORECLR_PROFILER = $CORECLR_PROFILER

# $OTEL_DOTNET_AUTO_HOME/instrument.sh dotnet run


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ENV TEST_VAR_STAGE2 notLoading   # this proves only stage 1 runs during DEBUG
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