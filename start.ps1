
dotnet build --configuration Debug -o .\bin\live_debug
$Env:ASPNETCORE_ENVIRONMENT = "Development"
$Env:ASPNETCORE_URLS = "http://*:8098"
dotnet .\bin\live_debug\DenizenMetaWebsite.dll
