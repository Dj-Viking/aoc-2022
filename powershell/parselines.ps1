
Function Get-InputLines([String]$theInput) {
    [String[]]$lines = @()
    $lines = $theInput.Split([System.Environment]::NewLine, [System.StringSplitOptions]::RemoveEmptyEntries)
    return $lines
}