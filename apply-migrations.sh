#!/bin/bash
dotnet restore
dotnet ef migrations bundle -p Infrastructure/Infrastructure.csproj -s Api/Api.csproj -c Infrastructure.Database.ApplicationDbContext --self-contained -o efbundle -r linux-arm64
./efbundle --connection "$(< secrets/ConnectionStringsOptions__DefaultConnection)"
rm efbundle