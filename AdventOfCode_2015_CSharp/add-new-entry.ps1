param(
    [Parameter(Mandatory=$true)]
    [int]$Year,
    [Parameter(Mandatory=$true)]
    [ValidatePattern('^\d{2}$')]
    [string]$DayNumber
)

# Get the script directory
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path

# Calculate day without leading zeros
$DayWithoutZeros = [int]$DayNumber

# Define paths
$sourceFolder = Join-Path $scriptDir "day00"
$targetFolder = Join-Path $scriptDir "day$DayNumber"
$programCsPath = Join-Path $scriptDir "Program.cs"
$templatePath = Join-Path $sourceFolder "daytemplate.txt"

# Check if source folder exists
if (-not (Test-Path $sourceFolder)) {
    Write-Error "Source folder 'day00' not found at: $sourceFolder"
    exit 1
}

# Check if target folder already exists
if (Test-Path $targetFolder) {
    Write-Warning "Target folder 'day$DayNumber' already exists. Overwriting files..."
} else {
    Write-Host "Creating new folder: day$DayNumber"
    New-Item -ItemType Directory -Path $targetFolder -Force | Out-Null
}

# Copy the entire day00 folder to dayXX, excluding daytemplate.txt
Write-Host "Copying files from day00 to day$DayNumber..."
Get-ChildItem -Path $sourceFolder -Recurse | Where-Object { $_.Name -ne "daytemplate.txt" } | ForEach-Object {
    $targetPath = $_.FullName -replace [regex]::Escape($sourceFolder), $targetFolder
    if ($_.PSIsContainer) {
        New-Item -ItemType Directory -Path $targetPath -Force | Out-Null
    } else {
        Copy-Item -Path $_.FullName -Destination $targetPath -Force
    }
}

# Create an empty dayXX.txt file
$newDayFile = Join-Path $targetFolder "day$DayNumber.txt"
New-Item -ItemType File -Path $newDayFile -Force | Out-Null
Write-Host "Created empty file: day$DayNumber.txt"

# Get all files in the new folder (excluding directories)
$files = Get-ChildItem -Path $targetFolder -File -Recurse

# Replace all occurrences of "00" with the day number in file contents
Write-Host "Replacing '00' with '$DayNumber' in file contents..."
foreach ($file in $files) {
    $content = Get-Content -Path $file.FullName -Raw -Encoding UTF8
    $newContent = $content -replace '00', $DayNumber
    Set-Content -Path $file.FullName -Value $newContent -Encoding UTF8 -NoNewline
}

# Rename files that contain "00" in their names
Write-Host "Renaming files that contain '00'..."
$filesToRename = Get-ChildItem -Path $targetFolder -File -Recurse | Where-Object { $_.Name -match '00' }
foreach ($file in $filesToRename) {
    $newName = $file.Name -replace '00', $DayNumber
    $newPath = Join-Path $file.DirectoryName $newName
    Rename-Item -Path $file.FullName -NewName $newName -Force
    Write-Host "  Renamed: $($file.Name) -> $newName"
}

# Read and process the template content
if (Test-Path $templatePath) {
    Write-Host "Reading template content..."
    $templateContent = Get-Content -Path $templatePath -Raw -Encoding UTF8
    
    # Replace 00 with XX, YYYY with year, and DD with day without leading zeros
    $modifiedTemplate = $templateContent -replace '00', $DayNumber
    $modifiedTemplate = $modifiedTemplate -replace 'YYYY', $Year
    $modifiedTemplate = $modifiedTemplate -replace 'DD', $DayWithoutZeros
    
    # Append to Program.cs
    if (Test-Path $programCsPath) {
        Write-Host "Appending template to Program.cs..."
        
        # Read existing content and add newlines before appending
        $existingContent = Get-Content -Path $programCsPath -Raw -Encoding UTF8
        
        # Ensure there's a newline before adding the template
        if (-not $existingContent.EndsWith("`n")) {
            $existingContent += "`n"
        }
        
        # Add the new day content with proper formatting
        $newProgramContent = $existingContent + "`n" + $modifiedTemplate
        
        Set-Content -Path $programCsPath -Value $newProgramContent -Encoding UTF8 -NoNewline
        Write-Host "Successfully appended template to Program.cs"
    } else {
        Write-Warning "Program.cs not found at: $programCsPath"
    }
} else {
    Write-Warning "Template file not found at: $templatePath"
}

Write-Host "`nDone! Day$DayNumber has been created successfully." -ForegroundColor Green
Write-Host "`nReminder: Don't forget to add 'using AdventOfCode_2015_CSharp.day$DayNumber;' to Program.cs" -ForegroundColor Yellow