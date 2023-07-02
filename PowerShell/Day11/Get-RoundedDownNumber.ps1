Function Get-RoundedDownNumber([System.Decimal]$Number) {
    # Write-Host "[DEBUG]: input number is => $Number is type $($Number.GetType())" -ForegroundColor Yellow
    
    [System.String]$numstr = $Number.ToString();
    # Write-Host "[DEBUG]: number str => $numstr is type $($numstr.GetType())" -ForegroundColor Yellow
    
    [System.Array]$splitstr = $numstr.Split(".");
    # Write-Host "[DEBUG]: number str => $splitstr is type $($splitstr.GetType())" -ForegroundColor Yellow
    # Write-Host "[DEBUG]: rounded down str => $($splitstr[0]) is type $(([System.Int64]$splitstr[0]).GetType())" -ForegroundColor Yellow
    return [System.Int64]$splitstr[0];
}