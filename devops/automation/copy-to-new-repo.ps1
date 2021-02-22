param([string] $dest)
Push-Location "..\..\"

# $source = (Get-Location).Path
$exclude = @('.git','.github','bin','node_modules','obj','.vs', 'dist')
# Get-ChildItem "$($source)\*" -Recurse -Exclude $exclude | Copy-Item -Destination {Join-Path $dest $_.FullName.Substring($source.length)}

robocopy .\ $dest /s /xd $exclude
Pop-Location