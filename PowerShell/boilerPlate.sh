cat << EOF > $1.ps1

[String]\$answer1 = "answer goes here"
[String]\$answer2 = "answer goes here"
[String]\$myInput = ""

. ..\ReadInput.ps1

\$myInput = Read-Input \$args[1]

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