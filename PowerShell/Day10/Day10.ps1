
[String]$answer1 = "answer goes here"
[String]$myInput = ""
[String[]]$lines = @()

. ..\ReadInput.ps1
. ..\ParseLines.ps1

$myInput = Read-Input $args[1]
$lines = Get-InputLines $myInput

[Int64]$register = 1
[Int64]$sum = 0
[System.Collections.ArrayList]$sums = @()
[Int64]$cycles = 0

## NEW APPROACH just make an array list of the three pixel positions that the sprite will appear
## on any given register during a particular cycle

[System.Collections.ArrayList]$spriteVisibilityList = @()

for ($i = 1; $i -lt 241; $i++) {
    $spriteVisibilityList.Add("$($i),$($i + 1),$($i + 2)");
}

$spriteVisibilityList[38] = "39,40";
$spriteVisibilityList[39] = "40";
$spriteVisibilityList[78] = "79,80";
$spriteVisibilityList[79] = "80";
$spriteVisibilityList[118] = "119,120";
$spriteVisibilityList[119] = "120";
$spriteVisibilityList[158] = "159,160";
$spriteVisibilityList[159] = "160";
$spriteVisibilityList[198] = "199,200";
$spriteVisibilityList[199] = "200";
$spriteVisibilityList[238] = "239,240";
$spriteVisibilityList[239] = "240";


# "sprite vis list count => $($spriteVisibilityList.Count)"

# for ($i = 0; $i -lt $spriteVisibilityList.Count; $i++) {
#     Write-Host "list at index $i => $($spriteVisibilityList[$i])" -ForegroundColor Green
# }

$crtScreen = New-Object 'string[,]' 6, 40  # create a 6x40 array of strings

#initialize the 2d array to have 'dark pixels'
for ($col = 0; $col -lt 6; $col++) {
    for ($row = 0; $row -lt 40; $row++) {
        $crtScreen[$col, $row] = "."
    }
}

[Int64]$currentRow = 0
[Int64]$currentCol = 0

Function CheckCycle1([Int64]$cycles, [Int64]$register1) {

    $sigStr = 0

    switch ($cycles) {
        20 {
            $sigStr = 20 * $register1
            $sum = $sum + $sigStr
            $sums.Add($sum)
        }
        60 {
            $sigStr = 60 * $register1
            $sum = $sum + $sigStr
            $sums.Add($sum)
        }
        100 {
            $sigStr = 100 * $register1
            $sum = $sum + $sigStr
            $sums.Add($sum)
        }
        140 {
            $sigStr = 140 * $register1
            $sum = $sum + $sigStr
            $sums.Add($sum)
        }
        180 {
            $sigStr = 180 * $register1
            $sum = $sum + $sigStr
            $sums.Add($sum)
        }
        220 {
            $sigStr = 220 * $register1
            $sum = $sum + $sigStr
            $sums.Add($sum)
        }
    }
}

Function CheckCycle2([Int64]$cycles, [Int64]$register2) {

    #on each cycle - draw

    [System.Int64]$spritePixelOne = $null
    [System.Int64]$spritePixelTwo = $null
    [System.Int64]$spritePixelThree = $null

    if ($null -ne $spriteVisibilityList[$register2 - 1].Split(",")[0]) {
        $spritePixelOne   = [Int64]$spriteVisibilityList[$register2 - 1].Split(",")[0]
    }
    if ($null -ne $spriteVisibilityList[$register2 - 1].Split(",")[1]) {
        $spritePixelTwo   = [Int64]$spriteVisibilityList[$register2 - 1].Split(",")[1]
    }
    if ($null -ne $spriteVisibilityList[$register2 - 1].Split(",")[1]) {
        $spritePixelThree = [Int64]$spriteVisibilityList[$register2 - 1].Split(",")[2]
    }

    [System.Boolean]$isPixelLocatedAtRegisterDuringCycle = $false

    
    # print where the sprite is during these cycles only on the first row
    # assign a '#' into the $crtScreen[$currentRow][$cycles] where one of the three sprite pixels are located on the crt screen
    # only if the operation currently happening will yield the sprite in the current 
    # pixel to draw on the crt screen
    if ($cycles -ge 0 -and $cycles -le 40) {
        $currentRow = 0
        
        $isPixelLocatedAtRegisterDuringCycle = ($cycles -eq $spritePixelOne) -or ($cycles -eq $spritePixelTwo) -or ($cycles -eq $spritePixelThree)
        # Write-Host "is pixel located there $isPixelLocatedAtRegisterDuringCycle at cycle => $cycles at register => $register pixels $spritePixelOne $spritePixelTwo $spritePixelThree" -ForegroundColor Red
        
        # Write-Host "got pixels? with register $register2 - at cycle $cycles => $spritePixelOne $spritePixelTwo $spritePixelThree" -ForegroundColor Yellow
        
        if ($isPixelLocatedAtRegisterDuringCycle) {
            #draw!
            # Write-Host "Draw!" -ForegroundColor Magenta
            $crtScreen[$currentRow,($cycles - 1)] = "#";
        }   
    }
    elseif ($cycles -ge 41 -and $cycles -le 80) {
        $currentRow = 1
        $isPixelLocatedAtRegisterDuringCycle = (($cycles - 40) -eq $spritePixelOne) -or (($cycles - 40) -eq $spritePixelTwo) -or (($cycles - 40) -eq $spritePixelThree)
        # Write-Host "is pixel located there $isPixelLocatedAtRegisterDuringCycle at cycle => $cycles at register => $register pixels $spritePixelOne $spritePixelTwo $spritePixelThree" -ForegroundColor Red
        if ($isPixelLocatedAtRegisterDuringCycle) {
            #draw!
            # Write-Host "Draw!" -ForegroundColor Magenta
            $crtScreen[$currentRow,($cycles - 41)] = "#";
        }
    }
    elseif ($cycles -ge 81 -and $cycles -le 120) {
        $currentRow = 2
        $isPixelLocatedAtRegisterDuringCycle = (($cycles - 80) -eq $spritePixelOne) -or (($cycles - 80) -eq $spritePixelTwo) -or (($cycles - 80) -eq $spritePixelThree)

        if ($isPixelLocatedAtRegisterDuringCycle) {
            #draw!
            # Write-Host "Draw!" -ForegroundColor Magenta
            $crtScreen[$currentRow,($cycles - 81)] = "#";
        }
    }
    elseif ($cycles -ge 121 -and $cycles -le 160) {
        $currentRow = 3
        $isPixelLocatedAtRegisterDuringCycle = (($cycles - 120) -eq $spritePixelOne) -or (($cycles - 120) -eq $spritePixelTwo) -or (($cycles - 120) -eq $spritePixelThree)
        
        if ($isPixelLocatedAtRegisterDuringCycle) {
            #draw!
            # Write-Host "Draw!" -ForegroundColor Magenta
            $crtScreen[$currentRow,($cycles - 121)] = "#";
        }
    }
    elseif ($cycles -ge 161 -and $cycles -le 200) {
        $currentRow = 4
        $isPixelLocatedAtRegisterDuringCycle = (($cycles - 160) -eq $spritePixelOne) -or (($cycles - 160) -eq $spritePixelTwo) -or (($cycles - 160) -eq $spritePixelThree)

        if ($isPixelLocatedAtRegisterDuringCycle) {
            #draw!
            # Write-Host "Draw!" -ForegroundColor Magenta
            $crtScreen[$currentRow,($cycles - 161)] = "#";
        }
    }
    elseif ($cycles -ge 201 -and $cycles -le 240) {
        $currentRow = 5
        $isPixelLocatedAtRegisterDuringCycle = (($cycles - 200) -eq $spritePixelOne) -or (($cycles - 200) -eq $spritePixelTwo) -or (($cycles - 200) -eq $spritePixelThree)

        if ($isPixelLocatedAtRegisterDuringCycle) {
            #draw!
            # Write-Host "Draw!" -ForegroundColor Magenta
            $crtScreen[$currentRow,($cycles - 201)] = "#";
        }
    }
}

Function Debug-Screen($screen) {
    $iter = 0;
    foreach ($row in $screen) {
        $iter = $iter + 1
        Write-Host $row -NoNewline -ForegroundColor Green
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
            # Write-Host "noop instruction has cycle $cycles at register $register" -ForegroundColor Green
            CheckCycle1 $cycles $register 
            continue
        } 
        elseif ($instruction -eq "addx") {
            for ($i = 0; $i -lt 2; $i++) {
                $cycles = $cycles + 1
                # Write-Host "addx instruction has cycle $cycles at register $register" -ForegroundColor Yellow

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

    Write-Host "crt screen"

    Debug-Screen $crtScreen

    foreach ($line in $lines) {

        $instruction = $line.Split(" ")[0]
        $value = $line.Split(" ")[1]

        if ($instruction -eq "noop") {
            $_cycles = $_cycles + 1
            $currentCol = $currentCol + 1

            CheckCycle2 $_cycles $_register #draw a pixel if sprite is located on the screen in the current cycle ( column )

            continue
        }
        elseif ($instruction -eq "addx") {
            
            for ($i = 0; $i -lt 2; $i++) {
                $_cycles = $_cycles + 1
                $currentCol = $currentCol + 1
 
                CheckCycle2 $_cycles $_register #draw a pixel if sprite is located on the screen in the current cycle ( column )
                
                if ($i -eq 1) { # shift the register
                    $_register = $_register + [Int]$value
                }

            }
        }
    }
    

    Write-Host "[INFO]: solving part two..." -ForegroundColor Cyan
    Write-Host "[INFO]: part two answer is displayed below" -ForegroundColor Green
    Debug-Screen $crtScreen
    
}

PartOne
PartTwo
