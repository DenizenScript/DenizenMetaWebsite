#!/bin/bash
dotnet build --configuration Release -o ./bin/live_release
ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS=http://*:8098 dotnet ./bin/live_release/DenizenMetaWebsite.dll
