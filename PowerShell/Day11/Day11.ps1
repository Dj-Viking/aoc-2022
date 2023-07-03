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
    [System.Collections.ArrayList]$Items = @()
    [System.String]$Operation
    [System.String]$Test
    [System.String]$TestTrue
    [System.String]$TestFalse
    [System.Int64]$InspectCounter = 0

    [System.Void]Init(
        [System.Int16]$id, 
        [System.String]$items, 
        [System.String]$operation,
        [System.String]$test,
        [System.String]$testTrue, 
        [System.String]$testFalse
    ) {
        $this.Id = $id;

        [System.String]$itemsStr = $items.Split(
            ":", [System.StringSplitOptions]::RemoveEmptyEntries
        )[1].Split(
            ", ", [System.StringSplitOptions]::RemoveEmptyEntries
        );


        foreach ($numStr in $itemsStr.Split(" ")) {
            [System.Int64]$num = [System.Int64]$numStr;            
            $this.Items.Add($num) | Out-Null;
        }

        $this.Operation = $operation.Split(":")[1] | ForEach-Object {
            $_.Trim()
        };

        $this.Test = $test.Split(":")[1].Trim();

        $this.TestFalse = $testFalse.Split(":")[1].Trim();

        $this.TestTrue = $testTrue.Split(":")[1].Trim();
    }

    [System.Void]IncrementInspect() {
        $this.InspectCounter++;
    }

    static [System.Void]DebugMonkeyById(
        [System.Int16]$mkyId, 
        [System.Collections.ArrayList]$monkeyList
    ) {
        [Monkey]$mky = $monkeyList[$mkyId];

        
        Write-Host "[DEBUG MONKEY BY ID]: ~~~~ Debugging monkey BY ID $($mky.Id) ~~~~" -ForegroundColor Yellow
        Write-Host "[DEBUG MONKEY BY ID]: monkey items length $($mky.Items.Count)" -ForegroundColor Yellow
        Write-Host "[DEBUG MONKEY BY ID]: Items: [$($mky.Items | ForEach-Object { "$_,".Trim() })]" -ForegroundColor Yellow
        Write-Host "[DEBUG MONKEY BY ID]: Operation: $($mky.Operation)" -ForegroundColor Yellow
        Write-Host "[DEBUG MONKEY BY ID]: Test: $($mky.Test)" -ForegroundColor Yellow
        Write-Host "[DEBUG MONKEY BY ID]: TestFalse: $($mky.TestFalse)" -ForegroundColor Yellow
        Write-Host "[DEBUG MONKEY BY ID]: TestTrue: $($mky.TestTrue)" -ForegroundColor Yellow    
        Write-Host "[DEBUG MONKEY BY ID]: InspectCounter: $($mky.InspectCounter)" -ForegroundColor Yellow    
    }
     
    [System.Void]Debug() {
        Write-Host "[DEBUG MONKEY]: ~~~~ Debugging monkey $($this.Id) ~~~~" -ForegroundColor Yellow
        Write-Host "[DEBUG MONKEY]: Items: [$($this.Items)]" -ForegroundColor Yellow
        Write-Host "[DEBUG MONKEY]: Operation: $($this.Operation)" -ForegroundColor Yellow
        Write-Host "[DEBUG MONKEY]: Test: $($this.Test)" -ForegroundColor Yellow
        Write-Host "[DEBUG MONKEY]: TestFalse: $($this.TestFalse)" -ForegroundColor Yellow
        Write-Host "[DEBUG MONKEY]: TestTrue: $($this.TestTrue)" -ForegroundColor Yellow
        Write-Host "[DEBUG MONKEY]: InspectCounter: $($this.InspectCounter)" -ForegroundColor Yellow    
    }

    [System.Void]DivideMyWorryAfterInspect(
        [Me]$meRef
    ) {
        $meRef.Worry = Get-RoundedDownNumber($meRef.Worry / 3);
    }

    [System.Boolean]TestWorryIsDivisible([Me]$me, [System.Int64]$divisor) {
        [System.Boolean]$result = $me.Worry % $divisor -eq 0;
        return $result;
    }

    [System.Void]SetMyWorryDuringInspect(
        [Me]$meRef, 
        [System.String]$monkeyID,
        [System.Int64]$item,
        [System.Collections.ArrayList]$monkeyList
    ) {

        [Monkey]$mky = $monkeyList[$monkeyID];
        $meRef.Worry = $item;

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

[System.Int16]$monkeyIndex = 0;

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

        $MonkeyList.Add($monkey) | Out-Null;

        $monkeyIndex++;
    }
    else {
        continue;
    }
}

foreach ($monkey in $MonkeyList) {
    $monkey.Debug();
}

Function PartOne {

    Function ProceedTurn([Monkey]$mky) {
        #inspect
        [System.Int64]$itemsCount = $mky.Items.Count;

        while ($itemsCount -ne 0) {

            [System.Int64]$currentItem = $mky.Items[0];
            
            #update worry level based on item's level
            $mky.IncrementInspect();
            $mky.SetMyWorryDuringInspect($Me, $mky.Id, $currentItem, $MonkeyList);
            Write-Host "[DEBUG]: what is my worry now $($Me.Worry)" -ForegroundColor Yellow
            
            #divide worry
            $mky.DivideMyWorryAfterInspect($Me);
            Write-Host "[DEBUG]: what is my worry now after divide $($Me.Worry)" -ForegroundColor Yellow
            
            #test
            
            [System.Int16]$divisor = $mky.Test.Split(
                "test: divisible by", 
                [System.StringSplitOptions]::RemoveEmptyEntries
            )[0].Trim();
            
            [System.Boolean]$testResult = $mky.TestWorryIsDivisible($Me, $divisor);
            
            Write-Host "[DEBUG]: worry is divisible => $testResult" -ForegroundColor Yellow
            
            # perform true or false action after test assertion
            # remove from current monkey id and throw (item with new worry level) to monkey with id from the test action
            if ($testResult) {
                Write-Host "[INFO]: test result true THROW" -ForegroundColor Green
                # true action
                
                # remove from current monkey
                [Monkey]$currentMonkey = $MonkeyList[$mky.Id]
                $currentMonkey.Items.Remove($currentItem);
                $itemsCount--;

                #get the monkey to throw to
                [System.Int16]$monkeyIdToThrowTo = [System.Int16]$mky.TestTrue.Split(
                    "throw to monkey ",
                    [System.StringSplitOptions]::RemoveEmptyEntries
                )[0].Trim();
                    
                $MonkeyList[$monkeyIdToThrowTo].Items.Add($Me.Worry) | Out-Null
                    
                [Monkey]::DebugMonkeyById($mky.Id, $MonkeyList);
                [Monkey]::DebugMonkeyById($monkeyIdToThrowTo, $MonkeyList);
            }
            else {
                Write-Host "[INFO]: test result false THROW" -ForegroundColor Green
                # false action
                # remove from current monkey
                $MonkeyList[$mky.Id].Items.Remove($currentItem);
                $itemsCount--;
                
                #get the monkey to throw to
                [System.Int16]$monkeyIdToThrowTo = [System.Int16]$mky.TestFalse.Split(
                    "throw to monkey ",
                    [System.StringSplitOptions]::RemoveEmptyEntries
                )[0].Trim();

                $MonkeyList[$monkeyIdToThrowTo].Items.Add($Me.Worry) | Out-Null;
                
                [Monkey]::DebugMonkeyById($mky.Id, $MonkeyList);
                [Monkey]::DebugMonkeyById($monkeyIdToThrowTo, $MonkeyList);
            }
        }
    }

    for ($round = 0; $round -lt 20; $round++) {
        # for ($round = 0; $round -lt 1; $round++) {
        foreach ($monkey in $MonkeyList) {
        
            ProceedTurn $monkey;
         
            Write-Host "[DEBUG AFTER TURN]" -ForegroundColor Green
            foreach ($monkey in $MonkeyList) {
                $monkey.Debug();
            }
        
        }
    }

    [System.Collections.ArrayList]$inspectionCountList = $MonkeyList | ForEach-Object {
        [Monkey]$mky = $_;
        $mky.InspectCounter;
    }

    $inspectionCountList.Sort();

    Write-Host "inspect count list $($inspectionCountList)" -ForegroundColor Cyan

    $answer1 = $inspectionCountList[$inspectionCountList.Count - 1] * $inspectionCountList[$inspectionCountList.Count - 2];

    Write-Host "[INFO]: solving part one..." -ForegroundColor Cyan;
    Write-Host "[INFO]: part one answer is $answer1" -ForegroundColor Green;
}
Function PartTwo {
    Write-Host "[INFO]: solving part two..." -ForegroundColor Cyan;
    Write-Host "[INFO]: part two answer is $answer2" -ForegroundColor Green;
}

PartOne;
PartTwo;

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
<#
Monkey 0:
  Monkey inspects an item with a worry level of 79.
    Worry level is multiplied by 19 to 1501.
    Monkey gets bored with item. Worry level is divided by 3 to 500.
    Current worry level is not divisible by 23.
    Item with worry level 500 is thrown to monkey 3.
  Monkey inspects an item with a worry level of 98.
    Worry level is multiplied by 19 to 1862.
    Monkey gets bored with item. Worry level is divided by 3 to 620.
    Current worry level is not divisible by 23.
    Item with worry level 620 is thrown to monkey 3.
#>
