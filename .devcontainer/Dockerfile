# Use the official .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:9.0

# Set environment variables
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true \
    PATH="${PATH}:/home/vscode/.dotnet/tools"

# Install necessary packages
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    postgresql-client \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# Install .NET tools as root first
RUN dotnet tool install --global dotnet-ef && \
    dotnet tool install --global dotnet-aspnet-codegenerator

# Create and set working directory
WORKDIR /workspace

# Create vscode user and set permissions
RUN useradd -ms /bin/bash vscode && \
    mkdir -p /home/vscode/.dotnet/tools && \
    chown -R vscode:vscode /home/vscode

# Switch to vscode user
USER vscode