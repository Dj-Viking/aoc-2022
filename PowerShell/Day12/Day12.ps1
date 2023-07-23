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
        coords = [Point]::new()
        char   = ' '
    };
    [System.Collections.Hashtable]$Down = @{
        coords = [Point]::new()
        char   = ' '
    };
    [System.Collections.Hashtable]$Left = @{
        coords = [Point]::new()
        char   = ' '
    };
    [System.Collections.Hashtable]$Right = @{
        coords = [Point]::new()
        char   = ' '
    };

    [System.Void]Debug() {
        $(if ($null -ne $this.Up."coords") { 
                Write-Host "[DEBUG ADJ]: up =>    [$($this.Up."coords".X), $($this.Up."coords".Y)] | $($this.Up."char")" -ForegroundColor Yellow 
            })
        $(if ($null -ne $this.Down."coords") { 
                Write-Host "[DEBUG ADJ]: down =>  [$($this.Down."coords".X), $($this.Down."coords".Y)]  | $($this.Down."char")" -ForegroundColor Yellow 
            })
        $(if ($null -ne $this.Left."coords") { 
                Write-Host "[DEBUG ADJ]: left =>  [$($this.Left."coords".X), $($this.Left."coords".Y)] | $($this.Left."char")" -ForegroundColor Yellow 
            })
        $(if ($null -ne $this.Right."coords") { 
                Write-Host "[DEBUG ADJ]: right => [$($this.Right."coords".X), $($this.Right."coords".Y)]  | $($this.Right."char")" -ForegroundColor Yellow 
            })
    }
}

class Point {
    [System.Int64]$X = 0;
    [System.Int64]$Y = 0;

    [System.Void]Init($x, $y) {
        $this.X = $x;
        $this.Y = $y;
    }
}

class Me {
    # array of points
    [System.Collections.ArrayList]$Visited = @();
    [Point]$MyCoords = [Point]::new();
    [System.Char]$CurrentLevel = " ";
    # multi dimensional list of coordinate points
    <#
        which is the shortest, that is part of the final answer
        [
            path 1
            [    X, Y
                (0, 2), (2, 3) => end
            ]

            path 2
            [
                (0, 2), (2, 1) => end
            ]
        ]
    #>
    [System.Collections.ArrayList]$PossiblePaths = 
    # list of points lists
    @(
        # points list
        # @(
        #### points
        #### [Point], [Point]
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
    [System.Void]DebugVisited([Grid]$grid) {
        Write-Host "line number => $($MyInvocation.ScriptLineNumber)" -ForegroundColor Cyan
        Write-Host "[ME DEBUG]: my visited" -ForegroundColor Green
        foreach ($point in $this.Visited) {
            [Point]$pt = $point;
            Write-Host "[ME DEBUG VISITED]: visited point => $($pt.X), $($pt.Y)" -ForegroundColor Cyan
            Write-Host "[ME DEBUG VISITED]: visited level => $($grid.Rows[$pt.Y][$pt.X])" -ForegroundColor Cyan
        }
    }

    # can only move in the direction of one letter above, same, or below
    # gather all possible paths I can take - pick the shortest path to the highest point at E, 
    # the amount of steps on the shortest path is the answer
    [System.Void]GetAllPossiblePaths([Grid]$grid, [Point]$point) {
        
    }

    [System.Void]MoveLocation([Grid]$grid, [Point]$point) {
        $this.MyCoords.X = $point.X;
        $this.MyCoords.Y = $point.Y;
        $this.CurrentLevel = $grid.Rows[$point.Y][$point.X];
    }

    static [AdjacentHashMap]GetAdjacentLevelsFromPoint([Grid]$grid, [Point]$point) {

        [AdjacentHashMap]$adjMap = [AdjacentHashMap]::new(); 

        #up
        if (($point.X - 1) -ne -1) {
            $adjMap.Up."coords".X = $point.X - 1;
            $adjMap.Up."coords".Y = $point.Y;
            $adjMap.Up."char" = $grid.Rows[($point.Y)][($point.X - 1)];
        }
        else {
            $adjMap.Up."coords" = $null;
            $adjMap.Up."char" = $null;
        }
        #down
        if (($point.X + 1) -le $grid.Rows.Count) {
            $adjMap.Down."coords".X = $point.X + 1;
            $adjMap.Down."coords".Y = $point.Y;
            $adjMap.Down."char" = $grid.Rows[($point.Y)][$point.X + 1];
        }
        else {
            $adjMap.Down."coords" = $null;
            $adjMap.Down."char" = $null;
        }
        #left
        if (($point.Y - 1) -ne -1) {
            $adjMap.Left."coords".X = $point.X;
            $adjMap.Left."coords".Y = $point.Y - 1;
            $adjMap.Left."char" = $grid.Rows[($point.Y - 1)][$point.X];
        }
        else {
            $adjMap.Left."coords" = $null;
            $adjMap.Left."char" = $null;
        }
        #right
        if ($point.Y -le $grid.Rows[0].Count) {
            $adjMap.Right."coords".X = $point.X;
            $adjMap.Right."coords".Y = $point.Y + 1;
            $adjMap.Right."char" = $grid.Rows[($point.Y + 1)][$point.X]
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

            $this.Rows.Add($row) | Out-Null;
        }
    }
}

Function PartOne {

    [Grid]$Grid = [Grid]::new();
    [Me]$Me = [Me]::new();

    $Grid.Init($lines);

    $Grid.Debug();

    $Me.Init($Grid);

    #start out visited the starting point of my location
    $Me.DebugLocation();
    # dereference the point from the start
    [Point]$pt = [Point]::new();
    $pt.Init($Me.MyCoords.X, $Me.MyCoords.Y);
    
    $Me.Visited.Add($pt) | Out-Null;
    $Me.DebugVisited($Grid);

    [System.Int64]$step = 0;
    # while ($Me.CurrentLevel -ne "E") {
    while ($step -lt 4) {
        
        # get adjacent locations from my current location
        [AdjacentHashMap]$adj = [Me]::GetAdjacentLevelsFromPoint($Grid, $Me.MyCoords);
        $adj.Debug();
        # move to a location based on which adjacent location I can move to
        #down
        if (($null -ne $adj.Down."coords") `
                -and ($adj.Down."coords" -notin $Me.Visited) `
                -and ($adj.Down."char" -le $Me.CurrentLevel)
        ) {
            $Me.Visited.Add($adj.Down."coords") | Out-Null;
            $Me.MoveLocation($Grid, $adj.Down."coords");
            $step++;
            Write-Host "[DEBUG]: loop step $step" -ForegroundColor Yellow
            $Me.DebugLocation();
            $Me.DebugVisited($Grid);
            continue;
        }
        #left
        if (($null -ne $adj.Left."coords") `
                -and ($adj.Left."coords" -notin $Me.Visited) `
                -and ($adj.Left."char" -le $Me.CurrentLevel)
        ) {
            $Me.Visited.Add($adj.Left."coords") | Out-Null;
            $Me.MoveLocation($Grid, $adj.Left."coords");
            $step++;
            Write-Host "[DEBUG]: loop step $step" -ForegroundColor Yellow
            $Me.DebugLocation();
            $Me.DebugVisited($Grid);
            continue;
        }
        #right
        if (($null -ne $adj.Right."coords") `
                -and ($adj.Right."coords" -notin $Me.Visited) `
                -and ($adj.Right."char" -le $Me.CurrentLevel)
        ) {
            $Me.Visited.Add($adj.Right."coords") | Out-Null;
            $Me.MoveLocation($Grid, $adj.Right."coords");
            $step++;
            Write-Host "[DEBUG]: loop step $step" -ForegroundColor Yellow
            $Me.DebugLocation();
            $Me.DebugVisited($Grid);
            continue;
        }
        #up
        if (($null -ne $adj.Up."coords") `
                -and ($adj.Up."coords" -notin $Me.Visited) `
                -and ($adj.Up."char" -le $Me.CurrentLevel)
        ) {
            $Me.Visited.Add($adj.Up."coords") | Out-Null;
            $Me.MoveLocation($Grid, $adj.Up."coords");
            $step++;
            Write-Host "[DEBUG]: loop step $step" -ForegroundColor Yellow
            $Me.DebugLocation();
            $Me.DebugVisited($Grid);
            continue;
        }

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
