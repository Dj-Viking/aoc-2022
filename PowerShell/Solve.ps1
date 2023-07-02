param(
    [Parameter(Mandatory = $true, HelpMessage = "Please enter a day number from 1-25")]
    [System.String]$DayNumber,
    [Parameter(Mandatory = $true, HelpMessage = "Please enter an input file name - e.g. input or sample")]
    [System.String]$InputFilename
)


try {
    # SOLVE FILE HERE
    $file = ".\Day$DayNumber\Day$DayNumber.ps1";
    powershell -NoProfile -File $file -InputFile $InputFilename
}
catch {
    <#Do this if a terminating exception happens#>
    [System.Management.Automation.ErrorRecord]$err = $_;
    
    Write-Host "[ERROR]: an error has occurred" -ForegroundColor Red
    Write-Host "[ERROR]: something" -ForegroundColor Red
    Write-Host "[ERROR]: $($err.Exception)" -ForegroundColor Red
    Write-Host "[ERROR]: $($err.ScriptStackTrace)" -ForegroundColor Red
}
finally {
    <#Do this after the try block regardless of whether an exception occurred or not#>
    Write-Host "[INFO]: solve complete" -ForegroundColor Cyan
}