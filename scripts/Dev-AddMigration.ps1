param(
	[Parameter(Mandatory=$true)]
    [string]$MigrationName
)

$ErrorActionPreference = "Stop"
Set-StrictMode -version 3.0
Import-Module "$PSScriptRoot\DatabaseFunctions.psm1" -DisableNameChecking -Force

cls


$databaseName = "ProjectX"
$serverInstance = "localhost"
$serverUsername = "sa"
$serverPassword = "Welcome123"
$configuration = "Development"

AddMigration -MigrationName $MigrationName -DatabaseName $DatabaseName -ServerInstance $ServerInstance -ServerUsername $ServerUsername -ServerPassword $ServerPassword