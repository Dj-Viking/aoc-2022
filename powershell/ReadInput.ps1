Function Read-Input([String]$Filename, [String]$CallRoot) {
    [String]$inputDocument = ""
    Write-Host "[DEBUG]: what is input filename => $Filename" -ForegroundColor Yellow
    $inputDocument = Get-Content -Path "$CallRoot\$Filename.txt" -Raw
    
    return $inputDocument
}