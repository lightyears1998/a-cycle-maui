param (
	[Parameter(Mandatory=$true)][string]$VersionString
)

$csprojFilePath = './ACycle/ACycle.csproj'
$windowsAppManifestFilePath = './ACycle/Platforms/Windows/Package.appxmanifest'

$csprojFileContent = Get-Content -Path $csprojFilePath -Encoding UTF8
$windowsAppManifestFileContent = Get-Content -Path $windowsAppManifestFilePath -Encoding UTF8

# For .csporj file:
## 1) Increase the version number by 1.
$captures = $csprojFileContent | Select-String -Pattern '<ApplicationVersion>(.*)</ApplicationVersion>'
$versionNumber = $captures.Matches.Groups[1].Value

$pattern = '(?<OpenTag><ApplicationVersion>).*(?<ClosingTag></ApplicationVersion>)'
$template = '${OpenTag}' + ([int]$versionNumber + 1) + '${ClosingTag}'
$csprojFileContent = $csprojFileContent -Replace $pattern, $template

# For Windows App manifest:
## 1) Update the version string.
$pattern = '<Identity Name="(?<Name>.*)" Publisher="(?<Publisher>.*)" Version=".*" />'
$template = '<Identity Name="${Name}" Publisher="${Publisher}" Version="' + $VersionString + '" />'
$windowsAppManifestFileContent = $windowsAppManifestFileContent -Replace $pattern, $template

Set-Content -Path $csprojFilePath -Value $csprojFileContent -Encoding UTF8
Set-Content -Path $windowsAppManifestFilePath -Value $windowsAppManifestFileContent -Encoding UTF8
