param(
	[switch]$Clean,
    [switch]$Upgrade
)

$ErrorActionPreference = "Stop"
Set-StrictMode -version 3.0
Import-Module "$PSScriptRoot\DatabaseFunctions.psm1" -DisableNameChecking -Force

if(!$Clean -and !$Upgrade){
	Write-Host "ERROR: Must specify database build type [-Clean, -Upgrade]" -ForegroundColor red
    Exit(0)
}

cls

# https://patorjk.com/software/taag/#p=display&f=Graffiti&t=Type%20Something%20
$asciiArt = "
________          __        ___.                         
\______ \ _____ _/  |______ \_ |__ _____    ______ ____  
 |    |  \\__  \\   __\__  \ | __ \\__  \  /  ___// __ \ 
 |    |   \/ __ \|  |  / __ \| \_\ \/ __ \_\___ \\  ___/ 
/_______  (____  /__| (____  /___  (____  /____  >\___  >
        \/     \/          \/    \/     \/     \/     \/
"
Write-Host $asciiArt

$databaseName = "ProjectX"
$serverInstance = "localhost"
$serverUsername = "sa"
$serverPassword = "Welcome123"

if($Clean){
    Write-Host "> Starting Clean"
    DropAndRecreateDatabase -DatabaseName $databaseName -ServerInstance $serverInstance -ServerUsername $serverUsername -ServerPassword $serverPassword
    Write-Host "> Finished Clean"
    Write-Host ""
}

Write-Host "> Starting Applying Migrations"
ApplyMigrations -DatabaseName $databaseName -ServerInstance $serverInstance -ServerUsername $serverUsername -ServerPassword $serverPassword
Write-Host "> Finished Applying Migrations"
Write-Host ""