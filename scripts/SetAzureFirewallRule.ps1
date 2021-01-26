$ErrorActionPreference = "Stop"
Set-StrictMode -version 3.0

Import-Module -Name "AzureRM.Sql"

[CmdletBinding(DefaultParameterSetName = 'None')]
param
(
  [String] [Parameter(Mandatory = $true)] $ServerName,
  [String] [Parameter(Mandatory = $true)] $ResourceGroup,
  [String] $AzureFirewallName = "AzureWebAppFirewall"
)
$agentIP = (New-Object net.webclient).downloadstring("http://checkip.dyndns.com") -replace "[^\d\.]"
New-AzureRmSqlServerFirewallRule -ResourceGroupName $ResourceGroup -ServerName $ServerName -FirewallRuleName $AzureFirewallName -StartIPAddress $agentIp -EndIPAddress $agentIP