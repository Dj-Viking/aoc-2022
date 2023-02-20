
[String]$answer1 = "answer goes here"
[String]$answer2 = "answer goes here"
[String]$myInput = ""
[String[]]$lines = @()



. ..\ReadInput.ps1
. ..\ParseLines.ps1

$myInput = Read-Input $args[1]
$lines = Get-InputLines $myInput

[Int64]$register = 1
[Int64]$sum = 0
[System.Collections.ArrayList]$sums = [System.Collections.ArrayList]@()
[Int64]$sigStr = 0
[Int64]$cycles = 0

Function CheckCycle([Int64]$cycles, [Int64]$register) {

    $sigStr = 0

    switch($cycles) {
        20 {
            $sigStr = 20 * $register
            $sum = $sum + $sigStr
            $sums.Add($sum)
        }
        60 {
            $sigStr = 60 * $register
            $sum = $sum + $sigStr
            $sums.Add($sum)
        }
        100 {
            $sigStr = 100 * $register
            $sum = $sum + $sigStr
            $sums.Add($sum)
        }
        140 {
            $sigStr = 140 * $register
            $sum = $sum + $sigStr
            $sums.Add($sum)
        }
        180 {
            $sigStr = 180 * $register
            $sum = $sum + $sigStr
            $sums.Add($sum)
        }
        220 {
            $sigStr = 220 * $register
            $sum = $sum + $sigStr
            $sums.Add($sum)
        }
    }
}

Function PartOne {

    foreach ($item in $lines) {

        $instruction = $item.Split(" ")[0]
        $value = $item.Split(" ")[1]

        Write-Host "$instruction => $value"

        if ($instruction -eq "noop") {
            $cycles = $cycles + 1
            CheckCycle $cycles $register
            continue
        } 
        elseif ($instruction -eq "addx") {
            for ($i = 0; $i -lt 2; $i++) {
                $cycles = $cycles + 1

                CheckCycle $cycles $register

                if ($i -eq 1) {
                    $register = $register + [Int]$value
                }
            }
        }


    }

    $sumofSums = 0

    foreach ($i in $sums) {
        $sumofSums = $sumofSums + $i
    }

    $answer1 = $sumofSums

    Write-Host "[INFO]: solving part one..." -ForegroundColor Cyan
    Write-Host "[INFO]: part one answer is $answer1" -ForegroundColor Green
}
Function PartTwo {
    Write-Host "[INFO]: solving part two..." -ForegroundColor Cyan
    Write-Host "[INFO]: part two answer is $answer2" -ForegroundColor Green
}

PartOne
PartTwo
