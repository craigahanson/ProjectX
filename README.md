# ProjectX
New project for developing a .net core ecosystem within a docker container

[![Build Status](https://dev.azure.com/CraigHanson/ProjectX/_apis/build/status/craigahanson.ProjectX?branchName=master)](https://dev.azure.com/CraigHanson/ProjectX/_build/latest?definitionId=1&branchName=master)

dotnet run --project src/ProjectX.IdentityServer - runs on port 1001
dotnet run --project src/ProjectX.WebApi - runs on port 1002
dotnet run --project src/ProjectX.Blazor --urls='http://localhost:1003' - runs on port 1003
dotnet run --project src/ProjectX.BlazorServer - runs on port 1004
dotnet run --project src/ProjectX.Mvc - runs on port 1005