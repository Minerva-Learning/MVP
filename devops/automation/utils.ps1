function ToCamelCaseName([string] $name) {
    $ti = (Get-Culture).TextInfo;
    $ti.ToTitleCase($name.Trim()).Replace(" ", "")
}

function ToLowerCaseName([string] $name) {
    $name.ToLower().Replace(" ", "")
}

<#
.EXAMPLE
$arr = @{name="Value 1"; val=1}, @{name="Value 2"; val=2}
PromtNumberSelect -msg "Please select by typing number" -opts ($arr | ForEach-Object {$_.name})
#>
function PromtNumberSelect([string] $msg, [string[]] $opts) {
    $index = 0
    ($opts | ForEach-Object {"$($index++;$index). $_"} | Write-Host)
    $selectedIndex = (Read-Host -Prompt $msg) - 1
    return $selectedIndex
}

function ReplaceInFile([string] $file, [string] $ph, [string] $val) {
    (Get-Content $file) -replace [Regex]::Escape($ph), $val | Set-Content $file
}