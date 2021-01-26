$ErrorActionPreference = "Stop"
Set-StrictMode -version 3.0

Import-Module -Name "AzureRM.Sql"

param
(
  [String] [Parameter(Mandatory = $true)] $ServerName,
  [String] [Parameter(Mandatory = $true)] $ResourceGroup,
  [String] $AzureFirewallName = "AzureWebAppFirewall"
)
Remove-AzureRmSqlServerFirewallRule -ServerName $ServerName -FirewallRuleName $AzureFirewallName -ResourceGroupName $ResourceGroup