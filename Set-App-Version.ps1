param (
	[Parameter(Mandatory=$true)][string]$VersionString
)

$csprojFilePath = './ACycle/ACycle.csproj'
$windowsAppManifestFilePath = './ACycle/Platforms/Windows/Package.appxmanifest'

$csprojFileContent = Get-Content -Path $csprojFilePath -Encoding UTF8 
$windowsAppManifestFileContent = Get-Content -Path $windowsAppManifestFilePath -Encoding UTF8 

Set-Content -Path $csprojFilePath -Value $csprojFileContent -Encoding UTF8 
Set-Content -Path $windowsAppManifestFilePath -Value $windowsAppManifestFileContent -Encoding UTF8 
