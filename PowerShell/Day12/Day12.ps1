param(
    [Parameter(Mandatory = $true, HelpMessage = "Please enter an input filename")]
    [System.String]$InputFilename
)

[String]$answer1 = "answer goes here"
[String]$answer2 = "answer goes here"
[String]$myInput = ""

. $PSScriptRoot\..\ReadInput.ps1
. $PSScriptRoot\..\ParseLines.ps1

$myInput = Read-Input $InputFilename $PSScriptRoot
$lines = Get-InputLines $myInput

class AdjacentHashMap {
    [System.Array]$Up = @();
    [System.Array]$Down = @();
    [System.Array]$Left = @();
    [System.Array]$Right = @();

    [System.Void]Debug() {
        
    }
}

class Me {
    [System.Int64]$X = 0;
    [System.Int64]$Y = 0;
    [System.Char]$CurrentLevel = " ";
    [System.Collections.ArrayList]$PossiblePaths = @();

    [System.Void]Init([Grid]$grid) {
        :getStart for ($row = 0; $row -lt $grid.Rows.Count; $row++) {
            for ($col = 0; $col -lt $grid.Rows[$row].Count; $col++) {
                if ($grid.Rows[$row][$col] -eq "S") {
                    $this.X = $col;
                    $this.Y = $row;
                    $this.CurrentLevel = "a";
                    break getStart;
                }
            }
        }
    }

    [System.Void]DebugLocation() {
        Write-Host "[ME DEBUG]: my location => [$($this.X), $($this.Y)] current level => [$($this.CurrentLevel)]" -ForegroundColor Magenta
    }

    # can only move in the direction of one letter above or below
    # gather all possible paths I can take - pick the shortest path to the highest point at E, 
    # the amount of steps on the shortest path is the answer
    [System.Void]GetPossiblePaths([Grid]$grid) {
        
    }

    [AdjacentHashMap]GetAdjacentLevelsFromCurrent([Grid]$grid) {

        [AdjacentHashMap]$adjList = [AdjacentHashMap]::new(); 

        $ErrorActionPreference = 'Continue';

        #up
        if ($null -ne [System.Char]$grid.Rows[$this.X - 1][$this.Y]) {
            $adjList.Up = @(($this.X - 1), $this.Y)
        }
        #down
        if ($null -ne [System.Char]$grid.Rows[$this.X + 1][$this.Y]) {
            $adjList.Down = @(($this.X + 1), $this.Y)
        }
        #left
        if ($null -ne [System.Char]$grid.Rows[$this.X][$this.Y - 1]) {
            $adjList.Left = @($this.X, $this.Y - 1)
        }
        #right
        if ($null -ne [System.Char]$grid.Rows[$this.X][$this.Y + 1]) {
            $adjList.Right = @($this.X, $this.Y + 1)
        }

        $ErrorActionPreference = 'Stop';
        
        return $adjList;
        
    }
}

class Grid {
    [System.Collections.ArrayList]$Rows = @()

    [System.Void]Debug() {
        foreach ($row in $this.Rows) {
            Write-Host "[GRID DEBUG]: row => [$row]" -ForegroundColor Yellow
        }
    }

    [System.Void]Init([System.Array]$lines) {
        foreach ($line in $lines) {
            [System.String]$_line = $line;
            
            [System.Array]$splitLine = $_line.ToCharArray();

            [System.Collections.ArrayList]$row = @();

            foreach ($str in $splitLine) {
                $row.Add($str) | Out-Null;
            }

            $this.Rows.Add($row);
        }
    }
}

Function PartOne {

    [Grid]$Grid = [Grid]::new();
    [Me]$Me = [Me]::new();

    $Grid.Init($lines);

    $Grid.Debug();

    $Me.Init($Grid);

    $Me.DebugLocation();

    [AdjacentHashMap]$adjMap = $Me.GetAdjacentLevelsFromCurrent($Grid);


    Write-Host "[INFO]: solving part one..." -ForegroundColor Cyan
    Write-Host "[INFO]: part one answer is $answer1" -ForegroundColor Green
}
Function PartTwo {
    Write-Host "[INFO]: solving part two..." -ForegroundColor Cyan
    Write-Host "[INFO]: part two answer is $answer2" -ForegroundColor Green
}

PartOne
PartTwo
