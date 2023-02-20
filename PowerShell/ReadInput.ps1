[String]$env:input = ""

Function Read-Input([String]$type) {
    $env:input = Get-Content -Path ".\$type.txt" -Raw
    
    return $env:input
}