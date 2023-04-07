
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

[System.Collections.Hashtable]$spriteVisibilityTable = [System.Collections.Hashtable]@{}
#init hash table to store which columns the sprite will be visible
for ($i = 0; $i -lt 240; $i++) {
    if ($i % 3 -eq 0) {
        $spriteVisibilityTable[$i] = "$($i),$($i + 1),$($i + 2)"
    }
}

foreach ($Key in $spriteVisibilityTable.Keys) {
    "The value of '$Key' is: $($spriteVisibilityTable[$Key])"
}

$crtScreen = New-Object 'string[,]' 6, 40  # create a 6x40 array of strings
#initialize the 2d array to have 'dark pixels'
for ($col = 0; $col -lt 6; $col++) {
    for ($row = 0; $row -lt 40; $row++) {
        $crtScreen[$col, $row] = "."
    }
}

[Int64]$currentRow = 0
[Int64]$currentCol = 0

Function CheckCycle1([Int64]$cycles, [Int64]$register) {

    $sigStr = 0

    switch ($cycles) {
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

Function CheckCycle2([Int64]$cycles, [Int64]$register) {
    
    
    if ($cycles -ge 0 -and $cycles -le 40) {
        $currentRow = 0
        
    }
    elseif ($cycles -ge 41 -and $cycles -le 80) {
        $currentRow = 1
        
    }
    elseif ($cycles -ge 81 -and $cycles -le 120) {
        $currentRow = 2
        
    }
    elseif ($cycles -ge 121 -and $cycles -le 160) {
        $currentRow = 3
        
    }
    elseif ($cycles -ge 161 -and $cycles -le 200) {
        $currentRow = 4
        
    }
    elseif ($cycles -ge 201 -and $cycles -le 240) {
        $currentRow = 5
        
    }
}

Function Debug-Screen($screen) {
    $iter = 0;
    foreach ($row in $screen) {
        $iter = $iter + 1
        Write-Host $row -NoNewline
        if ($iter -eq 40) {
            $iter = 0
            Write-Host ""
        }
    }
}

Function PartOne {

    foreach ($item in $lines) {

        $instruction = $item.Split(" ")[0]
        $value = $item.Split(" ")[1]

        if ($instruction -eq "noop") {
            $cycles = $cycles + 1
            CheckCycle1 $cycles $register
            continue
        } 
        elseif ($instruction -eq "addx") {
            for ($i = 0; $i -lt 2; $i++) {
                $cycles = $cycles + 1

                CheckCycle1 $cycles $register
                
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

    $_cycles = 0
    $_register = 1

    $currentRow = 0
    $currentCol = 0

    for ($col = 0; $col -lt 6; $col++) {
        for ($row = 0; $row -lt 40; $row++) {
            $crtScreen[$col, $row] = "."
        }
    }

    Write-Host "crt screen"

    Debug-Screen $crtScreen

    foreach ($line in $lines) {

        $instruction = $line.Split(" ")[0]
        $value = $line.Split(" ")[1]

        if ($instruction -eq "noop") {
            $_cycles = $_cycles + 1
            $currentCol = $currentCol + 1

            CheckCycle2 $_cycles $_register

            continue
        }
        elseif ($instruction -eq "addx") {
            
            for ($i = 0; $i -lt 2; $i++) {
                $_cycles = $_cycles + 1
                $currentCol = $currentCol + 1

                CheckCycle2 $_cycles $_register
                
                if ($i -eq 1) {
                    $_register = $_register + [Int]$value
                }

            }
        }
    }
    

    Write-Host "[INFO]: solving part two..." -ForegroundColor Cyan
    Write-Host "[INFO]: part two answer is $answer2" -ForegroundColor Green
}

PartOne
PartTwo
