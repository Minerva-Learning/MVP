$subIndex = 0
$loginArray = az login | ConvertFrom-Json
$subNamesArr = $loginArray | Select-Object -Property name | ForEach-Object {$_.name} 
$subNamesArr | ForEach-Object {"$($subIndex++;$subIndex) $_"}

$selectedSubscription = (Read-Host -Prompt "Please select subscription by enterint it's number") - 1
$selectedSubName = $subNamesArr | Select-Object -Skip $selectedSubscription -First 1

Write-Host "`nYou have choosen '$selectedSubName'."
az account set --subscription $selectedSubName

Write-Host "Loading SQL servers firewall rules..."
$servers = az sql server list
$serverIds = $servers | ConvertFrom-Json | Select-Object -Property id | ForEach-Object {$_.id}
$rules = az sql server firewall-rule list --ids $serverIds
$nameToRemove = Read-Host -Prompt "Enter user identifier (email) you want to remove access for"
Write-Host "`nYou are about to remove next firewall rules:`n"
$firewallRulesToRemove = $rules | ConvertFrom-Json | % { $_ } | ? { $_.name -like "$nameToRemove*" }
$firewallRulesToRemove | % { "$($_.name)[$($_.startIpAddress)-$($_.endIpAddress)] in '$($_.resourceGroup)'" }
$answer = Read-Host -Prompt "Are you sure you want to delete above rules? (yes/no)"
if ($answer -eq "yes") {
    $idsToRemove = $firewallRulesToRemove | % {$_.id}
    az sql server firewall-rule delete --ids $idsToRemove > $null
}
Write-Host "Finished."