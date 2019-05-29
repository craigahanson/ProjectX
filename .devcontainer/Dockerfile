#-------------------------------------------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See https://go.microsoft.com/fwlink/?linkid=2090316 for license information.
#-------------------------------------------------------------------------------------------------------------

FROM mcr.microsoft.com/dotnet/core/sdk:latest

# Install git, process tools
RUN apt-get update \
    && apt-get -y install git \
    && apt-get -y install procps \
    && curl -sL https://deb.nodesource.com/setup_12.x | bash \
    && apt-get -y install nodejs \
    && npm install -g @angular/cli

# Clean up
RUN apt-get autoremove -y \
    && apt-get clean -y \
    && rm -rf /var/lib/apt/lists/*
