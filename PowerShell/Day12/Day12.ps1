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

    [System.Void]Init($pt) {
        $this.X = $pt.X;
        $this.Y = $pt.Y;
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
            Write-Host "[ME DEBUG VISITED]: visited point => $($pt.X), $($pt.Y) | visited level => $($grid.Rows[$pt.X][$pt.Y])" -ForegroundColor Cyan
        }
    }

    # can only move in the direction of one letter above, same, or below
    # gather all possible paths I can take - pick the shortest path to the highest point at E, 
    # the amount of steps on the shortest path is the answer
    [System.Void]GetAllPossiblePaths([Grid]$grid, [Point]$point) {
        
    }

    [System.Boolean]CanMove([System.Char]$char) {
        [System.Boolean]$result = $false;

        [System.Int64]$convertedChar = [System.Int64]([System.Char]$char);
        [System.Int64]$convertedMyChar = [System.Int64]([System.Char]$this.CurrentLevel);

        Write-Host "[DEBUG COMPARE]: converted char $($convertedChar) against my char $($convertedMyChar)" -ForegroundColor Magenta

        $result = ($convertedChar - $convertedMyChar -eq 0) -or ($convertedChar - $convertedMyChar -le 1);

        return $result;
    }

    # bias moving to a location that is higher than current by one.
    # if there isn't a location nearby that is higher favor going the same level
    # if there isn't a location nearby that is the same level then go down a level
    [System.Void]MoveLocation([Grid]$grid, [Point]$point) {
        $this.MyCoords.X = $point.X;
        $this.MyCoords.Y = $point.Y;
        $this.CurrentLevel = $grid.Rows[$point.X][$point.Y];
    }

    [System.Boolean]HasVisited([Point]$point) {
        [Boolean]$result = $false;

        foreach ($pt in $this.Visited) {
            if ($pt.X -eq $this.MyCoords.X -and $pt.Y -eq $this.MyCoords.Y) {
                $result = $true;
                break;
            }
        }

        return $result;
    }

    static [AdjacentHashMap]GetAdjacentLevelsFromPoint([Grid]$grid, [Point]$point) {

        [AdjacentHashMap]$adjMap = [AdjacentHashMap]::new(); 

        #up
        if (($point.X - 1) -ge 0) {
            $adjMap.Up."coords".X = $point.X - 1;
            $adjMap.Up."coords".Y = $point.Y;
            $adjMap.Up."char" = $grid.Rows[($point.X - 1)][($point.Y)];
        }
        else {
            $adjMap.Up."coords" = $null;
            $adjMap.Up."char" = $null;
        }
        #down
        if (($point.X + 1) -le $grid.Rows.Count) {
            $adjMap.Down."coords".X = $point.X + 1;
            $adjMap.Down."coords".Y = $point.Y;
            $adjMap.Down."char" = $grid.Rows[($point.X + 1)][($point.Y)];
        }
        else {
            $adjMap.Down."coords" = $null;
            $adjMap.Down."char" = $null;
        }
        #left
        if (($point.Y - 1) -ge 0) {
            $adjMap.Left."coords".X = $point.X;
            $adjMap.Left."coords".Y = $point.Y - 1;
            $adjMap.Left."char" = $grid.Rows[($point.X)][($point.Y - 1)];
        }
        else {
            $adjMap.Left."coords" = $null;
            $adjMap.Left."char" = $null;
        }
        #right
        if ($point.Y -le $grid.Rows[0].Count) {
            $adjMap.Right."coords".X = $point.X;
            $adjMap.Right."coords".Y = $point.Y + 1;
            $adjMap.Right."char" = $grid.Rows[($point.X)][($point.Y + 1)]
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

    [System.Void]Debug(
        [Me]$me
    ) {
        for ($row = 0; $row -lt $this.Rows.Count; $row++) {
            for ($col = 0; $col -lt $this.Rows[$row].Count; $col++) {
                [Point]$pt = [Point]::new();
                $pt.X = $col;
                $pt.Y = $row;

                [System.Boolean]$my_curr_level_matches_point_in_grid = $me.CurrentLevel -eq $this.Rows[$row][$col];
                [System.Boolean]$my_curr_coords_matches_point_in_grid = $me.MyCoords.X -eq $row -and $me.MyCoords.Y -eq $col;
                [System.Boolean]$my_curr_coords_are_at_start = $me.MyCoords.X -eq 0 -and $me.MyCoords.Y -eq 0 -and $row -eq 0 -and $col -eq 0;
                
                if ($my_curr_coords_are_at_start) {
                    Write-Host " $($this.Rows[$row][$col]) " -NoNewline -ForegroundColor Green
                }
                elseif ($my_curr_level_matches_point_in_grid -and $my_curr_coords_matches_point_in_grid) {
                    Write-Host " $($this.Rows[$row][$col]) " -NoNewline -ForegroundColor Green
                }
                else {
                    Write-Host " $($this.Rows[$row][$col]) " -NoNewline -ForegroundColor Yellow
                }
            }
            Write-Host "";
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

Function Read-HostToIncrementStep() {
    Read-Host "Press enter to increment step";
}

Function PartOne {

    [Grid]$Grid = [Grid]::new();
    [Me]$Me = [Me]::new();

    $Grid.Init($lines);


    $Me.Init($Grid);

    Write-Host "current level at start $($Me.CurrentLevel) | $($Me.MyCoords.X), $($Me.MyCoords.Y)"
    
    # $Grid.Debug($Me);

    #start out visited the starting point of my location
    $Me.DebugLocation();
    # dereference the point from the start
    [Point]$pt = [Point]::new();
    $pt.Init($Me.MyCoords);

    $Me.Visited.Add($pt) | Out-Null;
    $Me.DebugVisited($Grid);

    [System.Int64]$step = 0;
    # while ($Me.CurrentLevel -ne "E") {
    while ($step -lt 4) {

        $Grid.Debug($Me);

        
        # get adjacent locations from my current location
        [AdjacentHashMap]$adj = [Me]::GetAdjacentLevelsFromPoint($Grid, $Me.MyCoords);
        $adj.Debug();
        # move to a location based on which adjacent location that I can move to
        #down
        if (($null -ne $adj.Down."coords") `
                -and ($Me.HasVisited($adj.Down."coords")) `
                -and ($Me.CanMove($adj.Down."char"))
        ) {
            $Me.Visited.Add($adj.Down."coords") | Out-Null;
            $Me.MoveLocation($Grid, $adj.Down."coords");
            $myInput = Read-HostToIncrementStep;
            if ([System.String]::IsNullOrEmpty($myInput)) {
                $step++;
            }
            Write-Host "[DEBUG]: loop step $step" -ForegroundColor Yellow
            $Me.DebugLocation();
            $Me.DebugVisited($Grid);
            continue;
        }
        #left
        if (($null -ne $adj.Left."coords") `
                -and ($Me.HasVisited($adj.Left."coords")) `
                -and ($Me.CanMove($adj.Left."char"))
        ) {
            $Me.Visited.Add($adj.Left."coords") | Out-Null;
            $Me.MoveLocation($Grid, $adj.Left."coords");
            $myInput = Read-HostToIncrementStep;
            if ([System.String]::IsNullOrEmpty($myInput)) {
                $step++;
            }
            Write-Host "[DEBUG]: loop step $step" -ForegroundColor Yellow
            $Me.DebugLocation();
            $Me.DebugVisited($Grid);
            continue;
        }
        #right
        if (($null -ne $adj.Right."coords") `
                -and ($Me.HasVisited($adj.Right."coords")) `
                -and ($Me.CanMove($adj.Right."char"))
        ) {
            $Me.Visited.Add($adj.Right."coords") | Out-Null;
            $Me.MoveLocation($Grid, $adj.Right."coords");
            $myInput = Read-HostToIncrementStep;
            if ([System.String]::IsNullOrEmpty($myInput)) {
                $step++;
            }
            Write-Host "[DEBUG]: loop step $step" -ForegroundColor Yellow
            $Me.DebugLocation();
            $Me.DebugVisited($Grid);
            continue;
        }
        #up
        if (($null -ne $adj.Up."coords") `
                -and ($Me.HasVisited($adj.Up."coords")) `
                -and ($Me.CanMove($adj.Up."char"))
        ) {
            $Me.Visited.Add($adj.Up."coords") | Out-Null;
            $Me.MoveLocation($Grid, $adj.Up."coords");
            $myInput = Read-HostToIncrementStep;
            if ([System.String]::IsNullOrEmpty($myInput)) {
                $step++;
            }
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
