#!/bin/bash
dotnet restore
dotnet build
ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS=http://*:8098 dotnet bin/Debug/net6.0/DenizenMetaWebsite.dll
