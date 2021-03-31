#!/bin/bash
dotnet restore
ASPNETCORE_ENVIRONMENT=Production ASPNETCORE_URLS=http://*:8098 dotnet run
