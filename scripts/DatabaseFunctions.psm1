$ErrorActionPreference = "Stop"
Set-StrictMode -version 3.0

Import-Module -Name "SqlServer"

function DropAndRecreateDatabase {
    param(
        [Parameter(Mandatory=$true)]
        [string]$DatabaseName,
        [Parameter(Mandatory=$true)]
        [string]$ServerInstance,
        [Parameter(Mandatory=$true)]
        [string]$ServerUsername,
        [Parameter(Mandatory=$true)]
        [string]$ServerPassword
    )

    Write-Host " > Dropping database"
    privateDropDatabase -DatabaseName $DatabaseName -ServerInstance $ServerInstance -ServerUsername $ServerUsername -ServerPassword $ServerPassword
    Write-Host " > Dropped Database"

    Write-Host " > Creating database"
    privateCreateDatabase -DatabaseName $DatabaseName -ServerInstance $ServerInstance -ServerUsername $ServerUsername -ServerPassword $ServerPassword
    Write-Host " > Created Database"
}

function ApplyMigrations {
    param(
        [Parameter(Mandatory=$true)]
        [string]$DatabaseName,
        [Parameter(Mandatory=$true)]
        [string]$ServerInstance,
        [Parameter(Mandatory=$true)]
        [string]$ServerUsername,
        [Parameter(Mandatory=$true)]
        [string]$ServerPassword,
        [Parameter(Mandatory=$true)]
        [string]$Configuration
    )

    Write-Host " > Applying Migrations"
    privateApplyMigrations -DatabaseName $DatabaseName -ServerInstance $ServerInstance -ServerUsername $ServerUsername -ServerPassword $ServerPassword -Configuration $Configuration
    Write-Host " > Applied Migrations"
}

function privateDropDatabase {
    param(
        [Parameter(Mandatory=$true)]
        [string]$DatabaseName,
        [Parameter(Mandatory=$true)]
        [string]$ServerInstance,
        [Parameter(Mandatory=$true)]
        [string]$ServerUsername,
        [Parameter(Mandatory=$true)]
        [string]$ServerPassword
    )

    $serverConnection = New-Object -TypeName Microsoft.SqlServer.Management.Common.ServerConnection -ArgumentList $ServerInstance, $ServerUsername, $ServerPassword
    $server = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Server -ArgumentList $serverConnection
    $server.ConnectionContext.Connect();

    $databaseToDrop = $server.Databases[$DatabaseName]
    if($databaseToDrop -ne $null){
        $databaseToDrop.Drop();
    }
}

function privateCreateDatabase {
    param(
        [Parameter(Mandatory=$true)]
        [string]$DatabaseName,
        [Parameter(Mandatory=$true)]
        [string]$ServerInstance,
        [Parameter(Mandatory=$true)]
        [string]$ServerUsername,
        [Parameter(Mandatory=$true)]
        [string]$ServerPassword
    )

    $serverConnection = New-Object -TypeName Microsoft.SqlServer.Management.Common.ServerConnection -ArgumentList $ServerInstance, $ServerUsername, $ServerPassword
    $server = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Server -ArgumentList $serverConnection

    $databaseToCreate = New-Object -TypeName Microsoft.SqlServer.Management.Smo.Database -Argumentlist $server, $DatabaseName
    $databaseToCreate.Create()
}

function privateApplyMigrations {
    param(
        [Parameter(Mandatory=$true)]
        [string]$DatabaseName,
        [Parameter(Mandatory=$true)]
        [string]$ServerInstance,
        [Parameter(Mandatory=$true)]
        [string]$ServerUsername,
        [Parameter(Mandatory=$true)]
        [string]$ServerPassword,
        [Parameter(Mandatory=$true)]
        [string]$Configuration
    )

    if($Configuration -eq "Development"){
        dotnet ef database update --project ..\src\ProjectX.Data.EntityFrameworkCore --startup-project ..\src\ProjectX.Data.EntityFrameworkCore -- "Server=$ServerInstance;Database=$DatabaseName;User ID=$ServerUsername;Password=$ServerPassword;Trusted_Connection=True;", 5
    } else { 
        dotnet ef database update --project src\ProjectX.Data.EntityFrameworkCore --startup-project src\ProjectX.Data.EntityFrameworkCore --msbuildprojectextensionspath obj/local -- "Server=$ServerInstance;Database=$DatabaseName;User ID=$ServerUsername;Password=$ServerPassword;Trusted_Connection=False;Encrypt=True;", 5
    }
}

Export-ModuleMember -Function DropAndRecreateDatabase
Export-ModuleMember -Function ApplyMigrations