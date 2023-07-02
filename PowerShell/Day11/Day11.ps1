param(
    [Parameter(Mandatory = $true, HelpMessage = "Please enter an input filename")]
    [System.String]$InputFilename
)

[String]$answer1 = "answer goes here"
[String]$answer2 = "answer goes here"
[String]$myInput = ""

. $PSScriptRoot\..\ReadInput.ps1
. $PSScriptRoot\..\ParseLines.ps1
. $PSScriptRoot\Get-RoundedDownNumber.ps1


$myInput = Read-Input $InputFilename $PSScriptRoot
[System.Array]$lines = Get-InputLines $myInput

[System.Collections.ArrayList]$MonkeyList = @();

# initialize the monkey stuff
# read from input files to allocate the monkeys and their items

class Me {
    [System.Int64]$Worry = 0
}

[Me]$Me = [Me]::new();

class Monkey {
    [System.Int16]$Id
    [System.String]$Items
    [System.String]$Operation
    [System.String]$Test
    [System.String]$TestTrue
    [System.String]$TestFalse

    [System.Void]Init(
        [System.Int16]$id, 
        [System.String]$items, 
        [System.String]$operation,
        [System.String]$test,
        [System.String]$testTrue, 
        [System.String]$testFalse
    ) {
        $this.Id = $id;

        $this.Items = $items.Split(":")[1] | ForEach-Object {
            $_.Replace(" ", "").Trim();
        };

        $this.Operation = $operation.Split(":")[1] | ForEach-Object {
            $_.Trim()
        };

        $this.Test = $test.Split(":")[1].Trim();

        $this.TestFalse = $testFalse.Split(":")[1].Trim();

        $this.TestTrue = $testTrue.Split(":")[1].Trim();
    }
     
    [System.Void]Debug() {
        Write-Host "[DEBUG MONKEY]: Debugging monkey $($this.Id)" -ForegroundColor Yellow
        Write-Host "[DEBUG MONKEY]: items: $($this.Items)" -ForegroundColor Yellow
        Write-Host "[DEBUG MONKEY]: operation: $($this.Operation)" -ForegroundColor Yellow
        Write-Host "[DEBUG MONKEY]: test: $($this.Test)" -ForegroundColor Yellow
        Write-Host "[DEBUG MONKEY]: test false: $($this.TestFalse)" -ForegroundColor Yellow
        Write-Host "[DEBUG MONKEY]: test true: $($this.TestTrue)" -ForegroundColor Yellow
    }

    [System.Void]DivideMyWorryAfterInspect(
        [Me]$meRef
    ) {
        $meRef.Worry = $meRef.Worry / 3;
    }

    [System.Void]SetMyWorryDuringInspect(
        [Me]$meRef, 
        [System.String]$monkeyID,
        [System.String]$item,
        [System.Collections.ArrayList]$monkeyList
    ) {

        [Monkey]$mky = $monkeyList[$monkeyID];
        $meRef.Worry = [System.Int64]$item;
        # perform math operation based on the monkey's operation
        [System.Array]$splitExpressionStr = $mky.Operation.Split(" = ", [System.StringSplitOptions]::RemoveEmptyEntries).Trim();
        [System.String]$operator = $splitExpressionStr[2];

        switch ($operator) {
            "+" {
                $meRef.Worry = $meRef.Worry + $(if ($splitExpressionStr[3] -cmatch "old") {
                        $meRef.Worry
                    }
                    else {
                        [System.Int64]$splitExpressionStr[3]
                    });
            }
            "-" {
                $meRef.Worry = $meRef.Worry - $(if ($splitExpressionStr[3] -cmatch "old") {
                        $meRef.Worry
                    }
                    else {
                        [System.Int64]$splitExpressionStr[3]
                    });
            }
            "*" {
                $meRef.Worry = $meRef.Worry * $(if ($splitExpressionStr[3] -cmatch "old") {
                        $meRef.Worry
                    }
                    else {
                        [System.Int64]$splitExpressionStr[3]
                    });
            }
            "/" {
                $meRef.Worry = $meRef.Worry / $(if ($splitExpressionStr[3] -cmatch "old") {
                        $meRef.Worry
                    }
                    else {
                        [System.Int64]$splitExpressionStr[3]
                    });
            }
        }
    }
}

$monkeyIndex = 0;

for ($line = 0; $line -lt $lines.Length; $line++) {

    [Monkey]$monkey = [Monkey]::new();

    [System.String]$startingItemsStr = "";
    [System.String]$operationStr = "";
    [System.String]$testStr = "";
    [System.String]$testTrue = "";
    [System.String]$testFalse = "";

    if ($lines[$line] -cmatch "Monkey") {
        $startingItemsStr = $lines[$line + 1];
        $operationStr = $lines[$line + 2];
        $testStr = $lines[$line + 3];
        $testTrue = $lines[$line + 4];
        $testFalse = $lines[$line + 5];
        
        $monkey.Init(
            $monkeyIndex, 
            $startingItemsStr, 
            $operationStr,
            $testStr,
            $testTrue,
            $testFalse
        );

        $MonkeyList.Add($monkey) | Out-Null

        $monkeyIndex++
    }
    else {
        continue;
    }
}

foreach ($monkey in $MonkeyList) {
    $monkey.Debug();
}

Function PartOne {


    Function MonkeyInspect() {

    }

    Function WorryDivide() {

    }

    Function MonkeyTest() {

    }

    Function ProceedRound([Monkey]$mky) {

    }

    foreach ($monkey in $MonkeyList) {
        ProceedRound $monkey
    }

    Write-Host "[INFO]: solving part one..." -ForegroundColor Cyan
    Write-Host "[INFO]: part one answer is $answer1" -ForegroundColor Green
}
Function PartTwo {
    Write-Host "[INFO]: solving part two..." -ForegroundColor Cyan
    Write-Host "[INFO]: part two answer is $answer2" -ForegroundColor Green
}

PartOne
PartTwo

<#
Monkey 0:
  Starting items: 79, 98 (worry level)
  Operation: new = old * 19 (new worry level = old worry level * 19 (after item was inspected by monkey))
  Test: divisible by 23 (how monkey uses worry level to decide which monkey to throw the item next)
    If true: throw to monkey 2 (what happens when test was true)
    If false: throw to monkey 3 (what happens when test was false)
#>

<#
steps:
    monkey inspect 79
    worry is calculated during inspect by the operation new = 79 * factor
    worry divided after inspection
        by 3 and rounded to nearest integer number 
    monkey test
#>
