dotnet restore
dotnet build
$Env:ASPNETCORE_ENVIRONMENT = "Development"
$Env:ASPNETCORE_URLS = "http://*:8098"
dotnet bin\Debug\net6.0\DenizenMetaWebsite.dll
