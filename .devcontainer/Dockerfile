# Use the official .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:9.0

# Set environment variables
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true \
    ASPNETCORE_ENVIRONMENT=Development \
    PATH="${PATH}:/root/.dotnet/tools"

# Install necessary packages
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    postgresql-client \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# Create and set working directory
WORKDIR /workspace

# Create vscode user and set permissions
RUN groupadd -g 1000 vscode && \
    useradd -u 1000 -g vscode -m vscode && \
    chown -R vscode:vscode /workspace

# Install .NET tools as root (they'll be available to all users)
RUN dotnet tool install --global dotnet-ef && \
    dotnet tool install --global dotnet-aspnet-codegenerator

# Switch to vscode user
USER vscode