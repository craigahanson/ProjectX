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

    Write-Host "Dropping database: $DatabaseName"
    privateDropDatabase -DatabaseName $DatabaseName -ServerInstance $ServerInstance -ServerUsername $ServerUsername -ServerPassword $ServerPassword
    Write-Host "Dropped Database: $DatabaseName"

    Write-Host "Creating database: $DatabaseName"
    privateCreateDatabase -DatabaseName $DatabaseName -ServerInstance $ServerInstance -ServerUsername $ServerUsername -ServerPassword $ServerPassword
    Write-Host "Created Database: $DatabaseName"
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

    Write-Host "Applying Migrations: $DatabaseName"
    privateApplyMigrations -DatabaseName $DatabaseName -ServerInstance $ServerInstance -ServerUsername $ServerUsername -ServerPassword $ServerPassword -Configuration $Configuration
    Write-Host "Applied Migrations: $DatabaseName"
}

function AddMigration {
    param(
        [Parameter(Mandatory=$true)]
        [string]$MigrationName,
        [Parameter(Mandatory=$true)]
        [string]$DatabaseName,
        [Parameter(Mandatory=$true)]
        [string]$ServerInstance,
        [Parameter(Mandatory=$true)]
        [string]$ServerUsername,
        [Parameter(Mandatory=$true)]
        [string]$ServerPassword,
        [Parameter(Mandatory=$true)]
        [ValidateSet('ProjextXContext','PersistedGrantDbContext','ConfigurationDbContext')]
        [string]$Context
    )

    Write-Host "Adding Migration to context: $Context"
    privateAddMigration -MigrationName $MigrationName `
        -DatabaseName $DatabaseName `
        -ServerInstance $ServerInstance `
        -ServerUsername $ServerUsername `
        -ServerPassword $ServerPassword `
        -Context $Context
    Write-Host "Added Migration to context: $Context"
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

    if($Configuration -eq "Development") {
        Write-Host "Updating Database: $DatabaseName using ProjextXContext"
        dotnet ef database update --project ..\src\ProjectX.Data.EntityFrameworkCore --startup-project ..\src\ProjectX.Data.EntityFrameworkCore -- "Server=$ServerInstance;Database=$DatabaseName;User ID=$ServerUsername;Password=$ServerPassword;Trusted_Connection=True;", 30
        Write-Host "Updated Database: $DatabaseName using ProjextXContext"

        Write-Host "Updating Database: $DatabaseName using PersistedGrantDbContext"
        dotnet ef database update --project ..\src\ProjectX.Authentication --startup-project ..\src\ProjectX.Authentication --context PersistedGrantDbContext -- "Server=$ServerInstance;Database=$DatabaseName;User ID=$ServerUsername;Password=$ServerPassword;Trusted_Connection=True;", 30
        Write-Host "Updated Database: $DatabaseName using PersistedGrantDbContext"
        
        Write-Host "Updating Database: $DatabaseName using ConfigurationDbContext"
        dotnet ef database update --project ..\src\ProjectX.Authentication --startup-project ..\src\ProjectX.Authentication --context ConfigurationDbContext -- "Server=$ServerInstance;Database=$DatabaseName;User ID=$ServerUsername;Password=$ServerPassword;Trusted_Connection=True;", 30
        Write-Host "Updated Database: $DatabaseName using ConfigurationDbContext"
    } else {
        dotnet ef database update --project src/ProjectX.Data.EntityFrameworkCore --startup-project src/ProjectX.Data.EntityFrameworkCore --connection "Server=$ServerInstance;Initial Catalog=$DatabaseName;User ID=$ServerUsername;Password=$ServerPassword" -- "Server=$ServerInstance;Initial Catalog=$DatabaseName;User ID=$ServerUsername;Password=$ServerPassword", 30
        dotnet ef database update --project src/ProjectX.Authentication --startup-project src/ProjectX.Authentication -c PersistedGrantDbContext --connection "Server=$ServerInstance;Initial Catalog=$DatabaseName;User ID=$ServerUsername;Password=$ServerPassword" -- "Server=$ServerInstance;Initial Catalog=$DatabaseName;User ID=$ServerUsername;Password=$ServerPassword", 30
        dotnet ef database update --project src/ProjectX.Authentication --startup-project src/ProjectX.Authentication -c ConfigurationDbContext --connection "Server=$ServerInstance;Initial Catalog=$DatabaseName;User ID=$ServerUsername;Password=$ServerPassword" -- "Server=$ServerInstance;Initial Catalog=$DatabaseName;User ID=$ServerUsername;Password=$ServerPassword", 30
    }
}

function privateAddMigration {
    param(
        [Parameter(Mandatory=$true)]
        [string]$MigrationName,
        [Parameter(Mandatory=$true)]
        [string]$DatabaseName,
        [Parameter(Mandatory=$true)]
        [string]$ServerInstance,
        [Parameter(Mandatory=$true)]
        [string]$ServerUsername,
        [Parameter(Mandatory=$true)]
        [string]$ServerPassword,
        [Parameter(Mandatory=$true)]
        [ValidateSet('ProjextXContext','PersistedGrantDbContext','ConfigurationDbContext')]
        [string]$Context
    )

    Switch ($Context){
        'ProjextXContext' { dotnet ef migrations add $MigrationName --project ..\src\ProjectX.Data.EntityFrameworkCore --startup-project ..\src\ProjectX.Data.EntityFrameworkCore -- "Server=$ServerInstance;Database=$DatabaseName;User ID=$ServerUsername;Password=$ServerPassword;Trusted_Connection=True;", 30 }
        'PersistedGrantDbContext' { dotnet ef migrations add $MigrationName --project ..\src\ProjectX.Authentication --startup-project ..\src\ProjectX.Authentication --context PersistedGrantDbContext --output-dir Data/Migrations/IdentityServer/PersistedGrantDb -- "Server=$ServerInstance;Database=$DatabaseName;User ID=$ServerUsername;Password=$ServerPassword;Trusted_Connection=True;", 30 }
        'ConfigurationDbContext' { dotnet ef migrations add $MigrationName --project ..\src\ProjectX.Authentication --startup-project ..\src\ProjectX.Authentication --context ConfigurationDbContext --output-dir Data/Migrations/IdentityServer/ConfigurationDb -- "Server=$ServerInstance;Database=$DatabaseName;User ID=$ServerUsername;Password=$ServerPassword;Trusted_Connection=True;", 30 }
    }
}

Export-ModuleMember -Function DropAndRecreateDatabase
Export-ModuleMember -Function ApplyMigrations
Export-ModuleMember -Function AddMigration