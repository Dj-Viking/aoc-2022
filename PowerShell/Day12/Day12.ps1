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
    [System.Collections.Hashtable]$Up = @{
        coords = @()
        char   = ' '
    };
    [System.Collections.Hashtable]$Down = @{
        coords = @()
        char   = ' '
    };
    [System.Collections.Hashtable]$Left = @{
        coords = @()
        char   = ' '
    };
    [System.Collections.Hashtable]$Right = @{
        coords = @()
        char   = ' '
    };

    [System.Void]Debug() {
        Write-Host "[DEBUG ADJ]: up =>    $($this.Up."coords") | $($this.Up."char")" -ForegroundColor Yellow
        Write-Host "[DEBUG ADJ]: down =>  $($this.Down."coords")  | $($this.Down."char")" -ForegroundColor Yellow
        Write-Host "[DEBUG ADJ]: left =>  $($this.Left."coords") | $($this.Left."char")" -ForegroundColor Yellow
        Write-Host "[DEBUG ADJ]: right => $($this.Right."coords")  | $($this.Right."char")" -ForegroundColor Yellow
    }
}

class Point {
    [System.Int64]$X = 0;
    [System.Int64]$Y = 0;
}

class Me {
    [Point]$MyCoords = [Point]::new();
    [System.Char]$CurrentLevel = " ";
    # multi dimensional list of coordinate points
    [System.Collections.ArrayList]$PossiblePaths = 
    # list of point lists
    @(
        # point list
        # @(
        #### points
        #### @()
        # )
    ); 

    [System.Void]Init([Grid]$grid) {
        :getStart for ($row = 0; $row -lt $grid.Rows.Count; $row++) {
            for ($col = 0; $col -lt $grid.Rows[$row].Count; $col++) {
                if ($grid.Rows[$row][$col] -eq "S") {
                    $this.MyCoords.X = $col;
                    $this.MyCoords.Y = $row;
                    $this.CurrentLevel = "a";
                    break getStart;
                }
            }
        }
    }

    [System.Void]DebugLocation() {
        Write-Host "[ME DEBUG]: my location => [$($this.MyCoords.X), $($this.MyCoords.Y)] current level => [$($this.CurrentLevel)]" -ForegroundColor Magenta
    }

    # can only move in the direction of one letter above, same, or below
    # gather all possible paths I can take - pick the shortest path to the highest point at E, 
    # the amount of steps on the shortest path is the answer
    [System.Void]GetPossiblePaths([Grid]$grid, [Point]$point) {
        
    }

    [System.Void]MoveLocation([Point]$point) {
        $this.MyCoords.X = $point.X;
        $this.MyCoords.Y = $point.Y;
    }

    static [AdjacentHashMap]GetAdjacentLevelsFromPoint([Grid]$grid, [Point]$point) {

        [AdjacentHashMap]$adjMap = [AdjacentHashMap]::new(); 

        #up
        if (($point.X - 1) -ne -1) {
            $adjMap.Up."coords" = @(($point.X - 1), $point.Y);
            $adjMap.Up."char" = $grid.Rows[($point.X - 1)][$point.Y];
        }
        else {
            $adjMap.Up."coords" = $null;
            $adjMap.Up."char" = $null;
        }
        #down
        if (($point.X + 1) -le $grid.Rows.Count) {
            $adjMap.Down."coords" = @(($point.X + 1), $point.Y);
            $adjMap.Down."char" = $grid.Rows[($point.X + 1)][$point.Y];
        }
        else {
            $adjMap.Down."coords" = $null;
            $adjMap.Down."char" = $null;
        }
        #left
        if (($point.Y - 1) -ne -1) {
            $adjMap.Left."coords" = @($point.X, ($point.Y - 1));
            $adjMap.Left."char" = $grid.Rows[$point.X][($point.Y - 1)];
        }
        else {
            $adjMap.Left."coords" = $null;
            $adjMap.Left."char" = $null;
        }
        #right
        if ($point.Y -le $grid.Rows[0].Count) {
            $adjMap.Right."coords" = @($point.X, ($point.Y + 1))
            $adjMap.Right."char" = $grid.Rows[$point.X][($point.Y + 1)]
        }
        else {
            $adjMap.Right."coords" = $null;
            $adjMap.Right."char" = $null;
        }

        return $adjMap;
        
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
    [Point]$Point = [Point]::new();

    $Grid.Init($lines);

    $Grid.Debug();

    $Me.Init($Grid);

    $Me.DebugLocation();

    #start 
    $Point.X = $Me.X;
    $Point.Y = $Me.Y;

    [AdjacentHashMap]$adjMap = [Me]::GetAdjacentLevelsFromPoint($Grid, $Point);

    $adjMap.Debug();

    Write-Host "[INFO]: solving part one..." -ForegroundColor Cyan
    Write-Host "[INFO]: part one answer is $answer1" -ForegroundColor Green
}
Function PartTwo {
    Write-Host "[INFO]: solving part two..." -ForegroundColor Cyan
    Write-Host "[INFO]: part two answer is $answer2" -ForegroundColor Green
}

PartOne
PartTwo
