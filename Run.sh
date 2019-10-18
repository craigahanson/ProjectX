dotnet run --project src/projectx.webapi &
dotnet run --project src/projectx.mvc &
dotnet watch --project src/projectx.blazor run --urls='http://0.0.0.0:4000;https://0.0.0.0:4001'