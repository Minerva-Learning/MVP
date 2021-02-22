. .\utils.ps1

$azTenants = az login
$subIndex = 0
$loginArray = $azTenants | ConvertFrom-Json
$subNamesArr = $loginArray | Select-Object -Property name | ForEach-Object {$_.name} 
$subNamesArr | ForEach-Object {"$($subIndex++;$subIndex) $_"}

$selectedSubscription = (Read-Host -Prompt "Please select subscription by enterint it's number") - 1
$selectedSubName = $subNamesArr | Select-Object -Skip $selectedSubscription -First 1

Write-Host "`nYou have choosen '$selectedSubName'."
az account set --subscription $selectedSubName

Write-Host "Obtainint list of available locations..."
az account list-locations | ConvertFrom-Json | ForEach-Object {"$($_.name) ($($_.displayName))"}
$selectedLocation = Read-Host -Prompt "`nPlease copy id of selected location and paste it(or leave blank for default 'centralus')"
$selectedLocation = if ($selectedLocation) { $selectedLocation } else { "centralus" }
az configure --defaults location=$selectedLocation
Write-Host "Default location is now set to '$selectedLocation'"

$rawProjName = Read-Host -Prompt "`nEnter project name in CamelCase (e.g. NewProject)"

$environments = @(
    @{name="Development"; env="dev"; netEnv="AzureDev"; ngAsset="azure-dev"},
    @{name="Staging"; env="staging"; netEnv="Staging"; ngAsset="staging"},
    @{name="UAT"; env="uat"; netEnv="UAT"; ngAsset="uat"},
    @{name="Production"; env="prod"; netEnv="Production"; ngAsset="production"})
$envId = PromtNumberSelect -msg "Select environment by entering it's number" -opts ($environments | ForEach-Object {$_.name})
$env = $environments[$envId]
$senv = $env.env
$envName = $env.netEnv

$sqlPassword = Read-Host -Prompt "Enter SQL server password if it's Production env, or leave empty for default password(access2web@)"
$sqlPassword = if ($sqlPassword) { $sqlPassword } else { "access2web@" }
$ccName = $rawProjName.Replace(" ", "") # ToCamelCaseName($rawProjName)
$lcName = ToLowerCaseName($rawProjName)
Write-Output "CamelCase name: '$ccName'`nFlat name:  '$lcName'"

$ccFullName = "$ccName-$senv"

Write-Host "`nCreating Resource Group..."

$projGroup = (az group create -n $ccFullName) | ConvertFrom-Json
$projGroupId = $projGroup.id
Write-Host "Resource Group with ID [$projGroupId] has been created."

Write-Host "`nStarting deployment, it may take several minutes to complete..."

# $deployParameters = 
#     "environment_name='$envName' " +
#     "sites_newproject_dev_name='$lcName-$senv' " + 
#     "sites_newproject_api_dev_name='$lcName-api-$senv' " +
#     "servers_newproject_sql_dev_name='$lcName-sql-$senv' " +
#     "components_newproject_dev_name='$lcName-$senv' " +
#     "serverfarms_newproject_api_plan_name='$lcName-api-$senv-plan' " +
#     "serverfarms_newproject_web_plan_name='$lcName-web-$senv-plan' " +
#     "components_newproject_api_dev_name='$lcName-api-$senv' " +
#     "actiongroups_application_insights_smart_detection_groupid='$projGroupId'";

$sqlServerName = "$lcName-sql-$senv"
$dbName = "$lcName-db-$senv"

# Write-Host "Deployment template parameters:`n$deployParameters`n"
$deployResult = (az deployment group create `
    --resource-group $ccFullName `
    --template-file deployment-template.json `
    --parameters environment_name=$envName `
    sites_newproject_dev_name=$lcName-$senv `
    sites_newproject_api_dev_name=$lcName-api-$senv `
    servers_newproject_sql_dev_name=$sqlServerName `
    sql_db_name=$dbName `
    components_newproject_dev_name=$lcName-$senv `
    serverfarms_newproject_api_plan_name=$lcName-api-$senv-plan `
    serverfarms_newproject_web_plan_name=$lcName-web-$senv-plan `
    components_newproject_api_dev_name=$lcName-api-$senv `
    sql_server_password=$sqlPassword `
    actiongroups_application_insights_smart_detection_groupid=$projGroupId)
$deployResult

$continueLocalPatch = Read-Host -Prompt "Do you want to continue and patch local app(yes/no)";
if ($continueLocalPatch.ToLower() -eq "yes") {
    function ReplaceConnectionStr([string] $file) {
        ReplaceInFile -file $file -ph "%%$($envName)__SQL_SERVER_URL%%" -val "$sqlServerName.database.windows.net"
        ReplaceInFile -file $file -ph "%%$($envName)__DB_NAME%%" -val $dbName
        ReplaceInFile -file $file -ph "%%$($envName)__DB_USER%%" -val "leanadmin"
        ReplaceInFile -file $file -ph "%%$($envName)__DB_PASS%%" -val $sqlPassword
    }

    Push-Location "..\..\"
    Push-Location "aspnet-core\src\Lean.Web.Host"
    ReplaceInFile -file "appsettings.json" -ph "LeanPlaceholderDb" -val "$($rawProjName)Db"
    
    Push-Location "..\Lean.Migrator"
    ReplaceInFile -file "appsettings.json" -ph "LeanPlaceholderDb" -val "$($rawProjName)Db"
    Pop-Location

    $appSettingsFileName = "appsettings.$envName.json"
    ReplaceConnectionStr -file $appSettingsFileName
    
    $clientUrl = "https://$lcName-$senv.azurewebsites.net"
    $apiUrl = "https://$lcName-api-$senv.azurewebsites.net"
    ReplaceInFile -file $appSettingsFileName -ph "%%$($envName)__API_URL%%" -val $apiUrl
    ReplaceInFile -file $appSettingsFileName -ph "%%$($envName)__CLIENT_URL%%" -val $clientUrl
    Pop-Location

    Push-Location "angular\src\assets"
    $ngAppConfigFileName = "appconfig.$($env.ngAsset).json"
    ReplaceInFile -file $ngAppConfigFileName -ph "%%$($envName)__API_URL%%" -val $apiUrl
    ReplaceInFile -file $ngAppConfigFileName -ph "%%$($envName)__CLIENT_URL%%" -val $clientUrl
    Pop-Location

    Push-Location "aspnet-core\src\Lean.Migrator"
    ReplaceConnectionStr -file $appSettingsFileName
    Pop-Location


    $renameSolutions = Read-Host -Prompt "Do you want to rename solutions(yes/no, default 'yes')";
    $renameSolutions = if ($renameSolutions) { $renameSolutions } else { "yes" }
    if ($renameSolutions.ToLower() -eq "yes") {
        Push-Location "aspnet-core"
        Rename-Item -Path "Lean.All.sln" -NewName "$($rawProjName).All.sln" -Force
        Rename-Item -Path "Lean.All.sln.DotSettings" -NewName "$($rawProjName).All.sln.DotSettings" -Force
        Rename-Item -Path "Lean.Web.sln" -NewName "$($rawProjName).Web.sln" -Force
        Rename-Item -Path "Lean.Web.sln.DotSettings" -NewName "$($rawProjName).Web.sln.DotSettings" -Force
        Rename-Item -Path "Lean.Mobile.sln" -NewName "$($rawProjName).Mobile.sln" -Force
        ReplaceInFile -file "src\Lean.Core\Web\WebContentFolderHelper.cs" -ph "Lean.Web.sln" -val "$($rawProjName).Web.sln"
        Pop-Location
    }

    Pop-Location
    Write-Host "Application configs have been successfully patched. Do not forget to verify repository changes and commit them."
}

Write-Host "Project has been initialized."