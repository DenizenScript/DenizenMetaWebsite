dotnet restore
$Env:ASPNETCORE_ENVIRONMENT = "Development"
$Env:ASPNETCORE_URLS = "http://*:8098"
dotnet run
