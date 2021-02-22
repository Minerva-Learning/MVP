param([string] $dest)
Push-Location "..\..\"

$exclude = @('.git','.github','bin','node_modules','obj','.vs', 'dist')

robocopy .\ $dest /s /xd $exclude /NFL /NDL /ETA
Pop-Location