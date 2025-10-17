param(
    [Parameter(Mandatory=$true)][int]$Year,
    [Parameter(Mandatory=$true)][int]$Day,
    [Parameter(Mandatory=$true)][ValidateSet("fsharp","fsx","fs","csharp","cs","python","py","powershell","ps1","txt")] [string]$Language,
    [string]$Root = ".",
    [switch]$Force
)

function Get-Extension {
    param($lang)
    switch ($lang.ToLower()) {
        "fsharp" { "fs" }
        "fs"     { "fs" }
        "fsx"    { "fsx" }
        "csharp" { "cs" }
        "cs"     { "cs" }
        "python" { "py" }
        "py"     { "py" }
        "powershell" { "ps1" }
        "ps1"    { "ps1" }
        default  { "txt" }
    }
}

# Resolve script directory (works when run via script or dot-sourced)
$scriptDir = if ($PSScriptRoot) { $PSScriptRoot } else { Split-Path -Parent $MyInvocation.MyCommand.Definition }

# If the caller left Root at the default ".", use the script directory as the base.
# If Root was supplied (not "."), resolve it to an absolute path.
if ($PSBoundParameters.ContainsKey('Root') -and $Root -ne ".") {
    try {
        $rootResolved = (Resolve-Path -Path $Root -ErrorAction Stop).ProviderPath
    } catch {
        Write-Warning "Could not resolve Root '$Root', falling back to script folder: $scriptDir"
        $rootResolved = $scriptDir
    }
} else {
    $rootResolved = $scriptDir
}

# normalize values
$dayPad = $Day.ToString("D2")
$ext = Get-Extension $Language
$yearDir = Join-Path -Path $rootResolved -ChildPath ("AdventOfCode_{0}" -f $Year)
$dayDir  = Join-Path -Path $yearDir -ChildPath ("day{0}" -f $dayPad)
$part01Dir = Join-Path -Path $dayDir -ChildPath "part01"
$part02Dir = Join-Path -Path $dayDir -ChildPath "part02"

$filesToCreate = @(
    (Join-Path $dayDir "test_input_01.txt"),
    (Join-Path $dayDir ("day{0}_input.txt" -f $dayPad)),
    (Join-Path $part01Dir ("day{0}_part01.{1}" -f $dayPad, $ext)),
    (Join-Path $part02Dir ("day{0}_part02.{1}" -f $dayPad, $ext))
)

# create directories (use .NET CreateDirectory which ensures all parts exist)
$dirs = @($yearDir, $dayDir, $part01Dir, $part02Dir)
foreach ($d in $dirs) {
    try {
        [System.IO.Directory]::CreateDirectory($d) | Out-Null
    } catch {
        Write-Warning "Failed to create directory '$d': $_"
    }
}

$created = @()
$skipped = @()

# use UTF8 without BOM for empty files
$enc = New-Object System.Text.UTF8Encoding($false)

foreach ($path in $filesToCreate) {
    if ((Test-Path $path) -and (-not $Force)) {
        $skipped += $path
        continue
    }

    # ensure parent directory exists (robust)
    $parent = Split-Path $path -Parent
    try {
        [System.IO.Directory]::CreateDirectory($parent) | Out-Null
    } catch {
        Write-Warning "Failed to ensure parent directory '$parent' exists: $_"
        continue
    }

    try {
        # write empty file (no default content)
        [System.IO.File]::WriteAllText($path, "", $enc)
        $created += $path
    } catch {
        Write-Warning "Failed to write file '$path': $_"
    }
}

if ($created.Count -gt 0) {
    "Created files:"
    $created | ForEach-Object { "  $_" }
}
if ($skipped.Count -gt 0) {
    "Skipped existing files (use -Force to overwrite):"
    $skipped | ForEach-Object { "  $_" }
}
"Done."