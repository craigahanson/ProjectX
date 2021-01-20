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

$databaseName = "ProjectX"
$serverInstance = "localhost"
$serverUsername = "sa"
$serverPassword = "Welcome123"

if($Clean){
    & ($PSScriptRoot + "\Database.ps1") -Clean -DatabaseName $databaseName -ServerInstance $serverInstance -ServerUsername $serverUsername -ServerPassword $serverPassword
}

if($Upgrade){
    & ($PSScriptRoot + "\Database.ps1") -Upgrade -DatabaseName $databaseName -ServerInstance $serverInstance -ServerUsername $serverUsername -ServerPassword $serverPassword
}