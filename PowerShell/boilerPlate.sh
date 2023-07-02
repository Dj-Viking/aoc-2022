mkdir Day$1

pushd Day$1

touch input.txt
touch sample.txt

cat << EOF > Day$1.ps1
param(
    [Parameter(Mandatory=\$true, HelpMessage="Please enter an input filename")]
    [System.String]\$InputFilename
)

[String]\$answer1 = "answer goes here"
[String]\$answer2 = "answer goes here"
[String]\$myInput = ""

. \$PSScriptRoot\..\ReadInput.ps1
. \$PSScriptRoot\..\ParseLines.ps1

\$myInput = Read-Input \$InputFilename \$PSScriptRoot
\$lines = Get-InputLines \$myInput

Function PartOne {
    Write-Host "[INFO]: solving part one..." -ForegroundColor Cyan
    Write-Host "[INFO]: part one answer is \$answer1" -ForegroundColor Green
}
Function PartTwo {
    Write-Host "[INFO]: solving part two..." -ForegroundColor Cyan
    Write-Host "[INFO]: part two answer is \$answer2" -ForegroundColor Green
}

PartOne
PartTwo
EOF

popd