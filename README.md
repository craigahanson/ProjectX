
# ProjectX
New project for developing a .net core ecosystem within a docker container

[![Build Status](https://dev.azure.com/CraigHanson/ProjectX/_apis/build/status/craigahanson.ProjectX?branchName=master)](https://dev.azure.com/CraigHanson/ProjectX/_build/latest?definitionId=1&branchName=master)

### Pre-requisites
dotnet tool install --global dotnet-ef  
Install-Module -Name SqlServer -AllowClobber  
Install-Module -Name AzureRM.sql -AllowClobber  
dotnet dev-certs https --trust  

### Run Apps
||Port|Command|
|-|-|-|
|ProjectX.Authentication|5001|dotnet run watch --project src/ProjectX.Authentication|
|ProjectX.Rest|1002|dotnet run watch --project src/ProjectX.Rest|
|ProjectX.Blazor|1003|dotnet run watch --project src/ProjectX.Blazor|

### Debug Blazor Web Assembly
chrome --remote-debugging-port=9222 --user-data-dir="C:\Users\craig\AppData\Local\Temp\blazor-chrome-debug" https://localhost:1003/version

### Entity Framework 
#### Scope sources
https://mehdi.me/ambient-dbcontext-in-ef6/  
https://github.com/mehdime/DbContextScope
