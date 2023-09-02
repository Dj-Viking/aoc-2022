param(
    [Parameter(Mandatory = $true, HelpMessage = "Please enter an input filename")]
    [System.String]$InputFilename
)

[String]$answer1 = "answer goes here"
[String]$answer2 = "answer goes here"
[String]$myInput = ""
$global:Directions = @("Down", "Up", "Left", "Right")


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

    [System.Int64]ConvertCharToInt([System.Char]$chr) {
        $result = 0;

        $result = [System.Int64]([System.Char]$chr);

        return $result;
    }

    [System.Void]MoveLocation([Point]$pt, [System.Char]$level) {
        $this.MyCoords.X = $pt.X;
        $this.MyCoords.Y = $pt.Y;
        $this.CurrentLevel = $level;
    }

    [System.Void]CheckAndMove([Point]$coordsToValidate, [AdjacentHashMap]$adj) {

        class Node {
            $coords = [Point]::new()
            $char = [System.String]" "
            $isUp = [Boolean]$false
            $isSame = [Boolean]$false
            $isDown = [Boolean]$false
            $direction = [String]"Right"
         
            Node($pt, $char, [String]$levelChange, [String]$direction) {
                $this.coords = $pt;
                $this.char = $char;
                $this.direction = $direction;
                switch ($levelChange) {
                    "up" {
                        $this.isUp = $true;
                        $this.isSame = $false;
                        $this.isDown = $false;
                        break;
                    }
                    "same" {
                        $this.isSame = $true;
                        break;
                    }
                    "down" {
                        $this.isDown = $true;
                        break;
                    }
                }
            }
        }

        class OptionDeterminer {
            $upIsOption = [Boolean]$false;
            $options = [System.Collections.ArrayList]@();

        }

        [OptionDeterminer]$optionDeterminer = [OptionDeterminer]::new();

        foreach ($direction in $global:Directions) {

            [Point]$adjcoords = $adj.$($direction)."coords";
            $adjCharInt = $this.ConvertCharToInt($adj.$($direction)."char");
            $adjChar = $adj.$($direction)."char";
            $myCharInt = $this.ConvertCharToInt($this.CurrentLevel);

            if ($null -eq $adjcoords) { continue; }

            # up in height
            if ($adjCharInt - $myCharInt -eq 1 -and !$this.HasVisited($adjcoords)) {
                # definitely move here if adj level is exactly one level up
                $optionDeterminer.upIsOption = $true;
                $optionDeterminer.options.Add([Node]::new($adjcoords, $adjChar, "up", $direction)) | Out-Null;
            }

            # only same height change
            if ($adjCharInt - $myCharInt -eq 0 -and !$this.HasVisited($adjcoords)) {
                # next best option is to just move to the same level
                if ($optionDeterminer.upIsOption) { break; } else {
                    # gather the options up if we haven't been able to determine if we can move up one level or not
                    $optionDeterminer.options.Add([Node]::new($adjcoords, $adjchar, "same", $direction)) | Out-Null;
                }
            }

            # only down in height
            if ($adjCharInt - $myCharInt -le -1 -and !$this.HasVisited($adjcoords)) {
                # only option is to go down if the available directions don't yield one level up or same level
                if ($optionDeterminer.upIsOption) { break; } else {
                    $optionDeterminer.options.Add([Node]::new($adjcoords, $adjChar, "down", $direction)) | Out-Null;
                }
            }

        }

        if ($optionDeterminer.upIsOption) {
            [Node]$node = $optionDeterminer.options | Where-Object { $_upIsOption }
            $this.MoveLocation($node.coords, $node.char);
            $this.Visited.Add($node.coords) | Out-Null;
        }
        elseif ($optionDeterminer.onlySameLevel) {
            [Node]$nodes = $optionDeterminer.options | Where-Object {
                [Node]$_node = [Node]$_;
    
                if ($_node.isSame) {
                    $_node;
                }
            }
            # might have more than one only same level found will have to just choose one i guess
            foreach ($option in $nodes) {
                [Node]$n = $option;
                
                
            }
            $this.MoveLocation($optionDeterminer.coords, $optionDeterminer.char);
            $this.Visited.Add($optionDeterminer.coords) | Out-Null;
        }
        elseif ($optionDeterminer.onlyDown) {
            $this.MoveLocation($optionDeterminer.coords, $optionDeterminer.char);
            $this.Visited.Add($optionDeterminer.coords) | Out-Null;
        }

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
            if ($pt.X -eq $point.X -and $pt.Y -eq $point.Y) {
                $result = $true;
                break;
            }
        }

        return $result;
    }

    static [AdjacentHashMap]GetAdjacentLevelsFromPoint([Grid]$grid, [Point]$point) {

        Write-Host "what is point checking adj [$($point.X), $($point.Y)]" -ForegroundColor Yellow;

        [AdjacentHashMap]$adjMap = [AdjacentHashMap]::new(); 

        #down
        if (($point.Y + 1) -le $grid.Rows.Count) {
            $adjMap.Down."coords".Y = $point.Y + 1;
            $adjMap.Down."coords".X = $point.X;
            $adjMap.Down."char" = $grid.Rows[($point.Y + 1)][($point.X)];
        }
        else {
            $adjMap.Down."coords" = $null;
            $adjMap.Down."char" = $null;
        }
        #up
        if (($point.Y - 1) -ge 0) {
            $adjMap.Up."coords".Y = $point.Y - 1;
            $adjMap.Up."coords".X = $point.X;
            $adjMap.Up."char" = $grid.Rows[($point.Y - 1)][($point.X)];
        }
        else {
            $adjMap.Up."coords" = $null;
            $adjMap.Up."char" = $null;
        }
        #right
        if (($point.X + 1) -le $grid.Rows[0].Count) {
            $adjMap.Right."coords".Y = $point.Y;
            $adjMap.Right."coords".X = $point.X + 1;
            $adjMap.Right."char" = $grid.Rows[($point.Y)][($point.X + 1)]
        }
        else {
            $adjMap.Right."coords" = $null;
            $adjMap.Right."char" = $null;
        }
        #left
        if (($point.X - 1) -ge 0) {
            $adjMap.Left."coords".Y = $point.Y;
            $adjMap.Left."coords".X = $point.X - 1;
            $adjMap.Left."char" = $grid.Rows[($point.Y)][($point.X - 1)];
        }
        else {
            $adjMap.Left."coords" = $null;
            $adjMap.Left."char" = $null;
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
    while ($Me.ConvertCharToInt($Me.CurrentLevel) -ne $Me.ConvertCharToInt('E')) {
        # while ($step -lt 31) {

        $Grid.Debug($Me);

        
        # get adjacent locations from my current location
        [AdjacentHashMap]$adj = [Me]::GetAdjacentLevelsFromPoint($Grid, $Me.MyCoords);
        $adj.Debug();

        :directions foreach ($direction in $global:Directions) {
            # skip direction if no coordinates available
            if ($null -eq $adj.$($direction)."coords") { continue; }

            # check if we have visited and we can move that direction
            # somehow check if the current direction we're checking is the best viable option for now
            $coords = $adj.$($direction)."coords";

            if (-not $Me.HasVisited(($coords))) {
                # validate where we should go
                $Me.CheckAndMove($coords, $adj);
                break directions;
            }
        }

        $myInput = Read-HostToIncrementStep;
                    
        if ([System.String]::IsNullOrEmpty($myInput)) {
            $step++;
        }
                    
        Write-Host "[DEBUG]: loop step $step" -ForegroundColor Yellow
        $Me.DebugLocation();
        $Me.DebugVisited($Grid);

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
