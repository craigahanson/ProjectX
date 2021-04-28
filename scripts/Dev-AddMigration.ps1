param(
	[Parameter(Mandatory=$true)]
    [string]$MigrationName,
    [Parameter(Mandatory=$true)]
    [ValidateSet('ProjextXContext','PersistedGrantDbContext','ConfigurationDbContext')]
    [string]$Context
)

$ErrorActionPreference = "Stop"
Set-StrictMode -version 3.0
Import-Module "$PSScriptRoot\DatabaseFunctions.psm1" -DisableNameChecking -Force

Clear-Host

$databaseName = "ProjectX"
$serverInstance = "localhost"
$serverUsername = "sa"
$serverPassword = "Welcome123"
$configuration = "Development"

AddMigration -MigrationName $MigrationName `
    -DatabaseName $DatabaseName `
    -ServerInstance $ServerInstance `
    -ServerUsername $ServerUsername `
    -ServerPassword $ServerPassword `
    -Context $Context