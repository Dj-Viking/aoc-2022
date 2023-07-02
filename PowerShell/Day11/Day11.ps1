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

$monkeyIndex = 0;

for ($line = 0; $line -lt $lines.Length; $line++) {
    $monkey = @{};
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
        
        $monkey."index" = $monkeyIndex;

        $monkey."operation" = $operationStr.Split(":")[1] | ForEach-Object {
            $_.Trim()
        }

        $monkey."testTrue" = $testTrue.Trim()
        $monkey."testFalse" = $testTrue.Trim()

        $monkey."items" = $startingItemsStr.Split(":")[1] | ForEach-Object {
            $_.Replace(" ", "");
        }

        $MonkeyList.Add($monkey) | Out-Null

        $monkeyIndex++
    }
    else {
        continue;
    }
}

foreach ($monkey in $MonkeyList) {
    Write-Host "[DEBUG]: look at monkey $($monkey."index") above: $(foreach($key in $monkey.Keys) { 
        Write-Host "key => $($key): value => $($monkey[$key])" -ForegroundColor Green
    })" -ForegroundColor Yellow
}

Function PartOne {


    Function MonkeyInspect() {

    }

    Function WorryDivide() {

    }

    Function MonkeyTest() {

    }

    Function ProceedRound() {

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
    monkey inspect
    worry divided after inspection
        by 3 and rounded to nearest integer number 
    monkey test
#>
