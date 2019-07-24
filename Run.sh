dotnet run --project src/projectx.webapi &
dotnet run --project src/projectx.mvc &
dotnet run --project src/projectx.angular &
dotnet run --project src/projectx.blazor --urls='http://0.0.0.0:4000;https://0.0.0.0:4001'