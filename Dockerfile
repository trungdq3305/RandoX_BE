# Stage 1: Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

# Stage 2: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution file and all project files
COPY RandoX.sln ./
COPY src/RandoX.API/RandoX.API.csproj ./src/RandoX.API/
COPY src/RandoX.Common/RandoX.Common.csproj ./src/RandoX.Common/
COPY src/RandoX.Data/RandoX.Data.csproj ./src/RandoX.Data/
COPY src/RandoX.Service/RandoX.Service.csproj ./src/RandoX.Service/

# Restore dependencies
RUN dotnet restore ./RandoX.sln

# Copy the rest of the source code
COPY src/ ./src/

# Build the project
WORKDIR /src/src/RandoX.API
RUN dotnet build RandoX.API.csproj -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish RandoX.API.csproj -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RandoX.API.dll"]
