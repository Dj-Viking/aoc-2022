using System.Runtime.CompilerServices;
namespace Day9
{
    class MainClass
    {
        public static bool _dg = true;
        public class Vec2
        {
            public int x { get; set; }
            public int y { get; set; }
            public Vec2(int _x, int _y)
            {
                this.x = _x;
                this.y = _y;
            }
        }
        public struct DirectionVec
        {
            public static Vec2 up { get { return new Vec2(-1, 0); } }       // -1,  0 up
            public static Vec2 upRight { get { return new Vec2(-1, 1); } }  // -1,  1 upright
            public static Vec2 upLeft { get { return new Vec2(-1, -1); } }  // -1, -1 upleft
            public static Vec2 down { get { return new Vec2(1, 0); } }      //  1,  0 down
            public static Vec2 downRight { get { return new Vec2(1, 1); } } //  1,  1 downright
            public static Vec2 downLeft { get { return new Vec2(1, -1); } } //  1, -1 downleft
            public static Vec2 left { get { return new Vec2(0, -1); } }     //  0, -1 left
            public static Vec2 right { get { return new Vec2(0, 1); } }     //  0,  1 right
        }
        public string input = "";
        public string[] _lines = new string[] { "" };
        public List<Dictionary<string, int>> OpMapList = new();
        public Graph graph = new Graph();
        public Rope rope = new Rope();
        public static int MAX_DIM = 0;
        public record Start(int x, int y);
        public static Start START = new Start(0, 0);
        public class OpName
        {
            public static readonly string R = "R";
            public static readonly string L = "L";
            public static readonly string U = "U";
            public static readonly string D = "D";
        }
        public static bool IsInBounds(int index, dynamic list)
        {
            if (list is List<List<string>>)
            {
                return (index >= 0) && (index < ((List<List<string>>)list).Count());
            }
            else if (list is List<string>)
            {
                return (index >= 0) && (index < ((List<string>)list).Count());
            }
            else
                return false;
        }
        public class Rope
        {
            public Head head = new Head();
            public Tail tail = new Tail();
            public Rope CreateCopy(Rope rope)
            {
                Rope newRope = new Rope();
                newRope.head = new Head();
                newRope.tail = new Tail();
                newRope.head.Coords = new Vec2(rope.head.Coords.x, rope.head.Coords.y);
                newRope.tail.Coords = new Vec2(rope.tail.Coords.x, rope.tail.Coords.y);

                newRope.tail.visited = new List<List<bool>>();

                for (int i = 0; i < rope.tail.visited.Count(); i++)
                {
                    List<bool> tempBoolList = new();
                    for (int j = 0; j < rope.tail.visited.Count(); j++)
                    {
                        tempBoolList.Add(rope.tail.visited[i][j]);
                    }
                    newRope.tail.visited.Add(tempBoolList);
                }

                return newRope;
            }
            public class Head
            {
                public Vec2 Coords = new Vec2(0, 0);
                public Head() { }
            }
            public class Tail
            {
                public Vec2 Coords = new Vec2(0, 0);
                public List<List<bool>> visited { get; set; } = new();
                public Tail() { }
            }
            public bool SetHeadLocation(Vec2 vec2, Graph graph)
            {
                int newCoordX = vec2.x += this.head.Coords.x;
                int newCoordY = vec2.y += this.head.Coords.y;

                if (!IsInBounds(newCoordX, graph.grid) || !IsInBounds(newCoordY, graph.grid[newCoordX]))
                {
                    // copy the old grid and rope into new ones to make room for the locations that we want to set
                    ResizeGrid(graph, this);
                }

                // apply new coordinates
                graph.ResetCurrentPlottedPoints(this);
                graph.DebugGraph();
                this.head.Coords.x = vec2.x < 0 ? 0 : vec2.x;
                this.head.Coords.y = vec2.y < 0 ? 0 : vec2.y;

                // TODO: figure out how to shift all of the tail's visited locations relative to the resizing grid
                if (vec2.y < 0)
                {
                    // shift all previous tail locations relative to the grid resize dimensions
                    for (int i = 1; i < this.tail.visited.Count() - 1; i++)
                    {
                        for (int j = 1; j < this.tail.visited[i].Count() - 1; j++)
                        {
                            if (this.tail.visited[i - 1][j - 1])
                            {
                                this.tail.visited[i - 1][j] = true;
                                this.tail.visited[i - 1][j - 1] = false;
                            }
                        }
                    }
                    this.tail.Coords.y += 1;
                }
                if (vec2.x < 0)
                {
                    // shift all previous tail locations relative to the grid resize dimensions
                    for (int i = 1; i < this.tail.visited.Count() - 1; i++)
                    {
                        for (int j = 1; j < this.tail.visited[i].Count() - 1; j++)
                        {
                            if (this.tail.visited[i - 1][j - 1])
                            {
                                this.tail.visited[i][j - 1] = true;
                                this.tail.visited[i - 1][j - 1] = false;
                            }
                        }
                    }
                    this.tail.Coords.x += 1;
                }

                // add new visited point for tail since the grid resized
                this.tail.visited[this.tail.Coords.x][this.tail.Coords.y] = true;
                graph.PlotVisited(this);
                return true;
            }
            public bool SetTailLocation(Vec2 vec2, Graph graph)
            {
                int newCoordX = vec2.x += this.tail.Coords.x;
                int newCoordY = vec2.y += this.tail.Coords.y;

                this.tail.Coords.x = vec2.x;
                this.tail.Coords.y = vec2.y;

                this.tail.visited[this.tail.Coords.x][this.tail.Coords.y] = true;
                return true;
            }
        }
        public class IsAdjacentResult
        {
            public bool isAdjacent = false;
            public Vec2? diffVec = null;
            public IsAdjacentResult(bool _isAdj, Vec2? diffVec)
            {
                this.isAdjacent = _isAdj;
                if (diffVec is not null)
                {
                    this.diffVec = new Vec2(diffVec.x, diffVec.y);
                }
            }
        }

        public class Graph
        {
            public List<List<string>> grid { get; set; } = new List<List<string>>();
            public List<List<string>> visited { get; set; } = new();
            public Graph() { }
            public Graph CreateCopy(Graph graph)
            {
                Graph newGraph = new Graph();
                newGraph.grid = new List<List<string>>();
                for (int i = 0; i < graph.grid.Count(); i++)
                {
                    List<string> tempStrList = new();
                    for (int j = 0; j < graph.grid[i].Count(); j++)
                    {
                        tempStrList.Add(graph.grid[i][j]);
                    }
                    newGraph.grid.Add(tempStrList);
                }
                newGraph.visited = new List<List<string>>();
                for (int i = 0; i < graph.visited.Count(); i++)
                {
                    List<string> tempStrList = new();
                    for (int j = 0; j < graph.visited[i].Count(); j++)
                    {
                        tempStrList.Add(graph.visited[i][j]);
                    }
                    newGraph.visited.Add(tempStrList);
                }
                return newGraph;
            }
            public bool isHeadUp(bool isDiffEqualToTwo, Rope rope)
            {
                if (isDiffEqualToTwo && rope.head.Coords.x == (rope.tail.Coords.x - 1))
                    return true;
                else return false;
            }
            public bool isHeadDown(bool isDiffEqualToTwo, Rope rope)
            {
                if (isDiffEqualToTwo && rope.head.Coords.x == (rope.tail.Coords.x + 1))
                    return true;
                else return false;
            }
            public bool isHeadLeft(bool isDiffEqualToTwo, Rope rope)
            {
                if (isDiffEqualToTwo && rope.head.Coords.y == (rope.tail.Coords.y - 1))
                    return true;
                else return false;
            }
            public bool isHeadRight(bool isDiffEqualToTwo, Rope rope)
            {
                if (isDiffEqualToTwo && rope.head.Coords.y == (rope.tail.Coords.y + 1))
                    return true;
                else return false;
            }

            public IsAdjacentResult IsTailAdjacentToHead(Rope rope)
            {
                int currentHeadX = rope.head.Coords.x;
                int currentHeadY = rope.head.Coords.y;
                int currentTailX = rope.tail.Coords.x;
                int currentTailY = rope.tail.Coords.y;

                Vec2 diffVec = new Vec2(
                    Math.Abs(currentHeadX - currentTailX),
                    Math.Abs(currentHeadY - currentTailY)
                );

                if ((diffVec.x == 1 && diffVec.y == 0) || (diffVec.y == 1 && diffVec.x == 0))
                    return new IsAdjacentResult(true, null);
                else if (diffVec.x == 1 && diffVec.y == 1)
                    return new IsAdjacentResult(true, null);
                else if (diffVec.x == 0 && diffVec.y == 0)
                    return new IsAdjacentResult(true, null);
                else
                    return new IsAdjacentResult(false, diffVec);
            }
            private void TrackTailVisited(Rope rope)
            {
                for (int i = 0; i < rope.tail.visited.Count(); i++)
                    for (int j = 0; j < rope.tail.visited[i].Count(); j++)
                        if (rope.tail.visited[i][j])
                            this.visited[i][j] = "#";

            }
            public void ResetCurrentPlottedPoints(Rope rope)
            {
                this.grid[rope.head.Coords.x][rope.head.Coords.y] = ".";
                this.grid[rope.tail.Coords.x][rope.tail.Coords.y] = ".";
            }
            public void PlotVisited(Rope rope)
            {
                this.ResetCurrentPlottedPoints(rope);
                // this.grid[START.x][START.y] = "s";
                this.grid[rope.tail.Coords.x][rope.tail.Coords.y] = "T";
                this.grid[rope.head.Coords.x][rope.head.Coords.y] = "H";
                this.TrackTailVisited(rope);
            }
            public void DebugGraph([CallerLineNumberAttribute] int lineNumber = 0)
            {
                // return;
                if (_dg)
                {

                    Console.WriteLine($"----- debugging graph {lineNumber}");
                    Console.Write("  ");
                    for (int k = 0; k < this.grid.Count(); k++)
                    {
                        if (k < 10) Console.Write($" {k} ");
                        else Console.Write($"{k} ");
                    }
                    Console.WriteLine();
                    for (int i = 0; i < this.grid.Count(); i++)
                    {
                        if (i < 10) Console.Write($"{i} ");
                        else Console.Write($"{i}");

                        for (int j = 0; j < this.grid[i].Count(); j++)
                        {
                            Console.Write($" {this.grid[i][j]} ");
                        }
                        Console.WriteLine();
                    }
                }
            }
            public void DebugTailVisited([CallerLineNumberAttribute] int lineNumber = 0)
            {
                // return;
                Console.WriteLine("----- debugging tail visited {0}", lineNumber);
                Console.Write("  ");
                for (int k = 0; k < this.visited.Count(); k++)
                {
                    if (k < 10) Console.Write($" {k} ");
                    else Console.Write($"{k} ");
                }
                Console.WriteLine();
                for (int i = 0; i < this.visited.Count(); i++)
                {
                    if (i < 10) Console.Write($"{i} ");
                    else Console.Write($"{i}");

                    for (int j = 0; j < this.visited[i].Count(); j++)
                    {
                        if (this.visited[i][j] == "#" || this.visited[i][j] == ".")
                        {
                            Console.Write($" {this.visited[i][j]} ");
                        }
                    }
                    Console.WriteLine();
                }
            }
        }

        public static void Main(string[] args)
        {
            new MainClass().Run(args);
        }
        public void Run(string[] args)
        {
            this.Init();
            this.GetInput(args[0]);
            this.PartOne();

            this.Init();
            this.GetInput(args[0]);
            this.PartTwo();
        }
        public void Init()
        {
            this.input = "";
            this._lines = new string[] { "" };
            this.OpMapList = new List<Dictionary<string, int>>();
            this.rope = new Rope();
            START = new Start(0, 0);
        }
        public void ParseOps()
        {
            foreach (string line in this._lines)
            {
                string opName = line.Split(" ")[0];
                int distance = int.Parse(line.Split(" ")[1]);

                Dictionary<string, int> opObj = new();
                opObj.Add(opName, distance);

                this.OpMapList.Add(opObj);
            }
        }
        public static void ResizeGrid(Graph graph, Rope rope)
        {
            Graph oldGraph = graph.CreateCopy(graph);
            Rope oldRope = rope.CreateCopy(rope);
            List<List<string>> strLists = new();
            List<List<bool>> boolLists = new();

            for (int i = 0; i < graph.grid.Count() + 1; i++)
            {
                List<string> tempStrList = new();
                List<bool> tempBoolList = new();
                for (int j = 0; j < graph.grid.Count() + 1; j++)
                {
                    tempStrList.Add(".");
                    tempBoolList.Add(false);
                }
                strLists.Add(tempStrList);
                boolLists.Add(tempBoolList);
            }

            graph.grid = strLists.Select(list => list.ToList()).ToList();
            graph.visited = strLists.Select(list => list.ToList()).ToList();
            rope.tail.visited = boolLists.Select(list => list.ToList()).ToList();

            // re apply visited coordinates from old to new
            for (int i = 0; i < oldGraph.grid.Count(); i++)
            {
                for (int j = 0; j < oldGraph.grid[i].Count(); j++)
                {
                    graph.visited[i][j] = oldGraph.grid[i][j];
                }
            }
            for (int i = 0; i < oldRope.tail.visited.Count(); i++)
            {
                for (int j = 0; j < oldRope.tail.visited.Count(); j++)
                {
                    rope.tail.visited[i][j] = oldRope.tail.visited[i][j];
                }
            }

        }
        public void InitGraph()
        {

            List<int> dimensionList = new();
            foreach (Dictionary<string, int> opObj in this.OpMapList)
            {
                foreach (string opName in opObj.Keys)
                {
                    dimensionList.Add(opObj[opName]);
                }
            }

            MAX_DIM = dimensionList.Max() + 1;

            List<List<string>> strLists = new();
            List<List<bool>> boolLists = new();

            for (int i = 0; i < MAX_DIM; i++)
            {
                List<string> tempList = new();
                List<bool> tempList2 = new();
                for (int j = 0; j < MAX_DIM; j++)
                {
                    tempList.Add(".");
                    tempList2.Add(false);
                }
                strLists.Add(tempList);
                boolLists.Add(tempList2);
            }

            this.graph.grid = strLists.Select(list => list.ToList()).ToList();
            this.graph.visited = strLists.Select(list => list.ToList()).ToList();
            this.rope.tail.visited = boolLists.Select(list => list.ToList()).ToList();
        }

        public void InitPositionsOnGraph()
        {
            int startX = MAX_DIM - 1;
            int startY = 0;
            START = new Start(startX, startY);
            this.rope.SetHeadLocation(new Vec2(startX, startY), this.graph);
            this.rope.SetTailLocation(new Vec2(startX, startY), this.graph);
        }
        public void MoveOperations()
        {
            string previousOp = "";
            foreach (Dictionary<string, int> opObj in this.OpMapList)
            {
                foreach (string opName in opObj.Keys)
                {
                    int moveAmount = opObj[opName];
                    bool tailDidMove = false;
                    if (opName == OpName.R)
                    {
                        for (int i = 0; i < moveAmount; i++)
                        {
                            // Console.WriteLine("---- moving RIGHT! this amount {0}", moveAmount);
                            if (this.rope.SetHeadLocation(DirectionVec.right, this.graph))
                            {
                                this.graph.ResetCurrentPlottedPoints(this.rope);
                                this.graph.PlotVisited(this.rope);
                                this.graph.DebugGraph();
                            }
                            else
                            {
                                this.graph.ResetCurrentPlottedPoints(this.rope);
                                this.graph.PlotVisited(this.rope);
                                this.graph.DebugGraph();
                                continue;
                            }

                            this.graph.ResetCurrentPlottedPoints(this.rope);

                            IsAdjacentResult adjResult = this.graph.IsTailAdjacentToHead(this.rope);

                            if (!adjResult.isAdjacent && adjResult.diffVec is not null)
                            {
                                tailDidMove = true;

                                if (adjResult.diffVec.y == 2 && adjResult.diffVec.x == 0)
                                {
                                    this.rope.SetTailLocation(DirectionVec.right, this.graph);
                                }
                                else if (this.graph.isHeadUp(adjResult.diffVec.y == 2, this.rope))
                                {
                                    this.rope.SetTailLocation(DirectionVec.upRight, this.graph);
                                }
                                else if (this.graph.isHeadDown(adjResult.diffVec.y == 2, this.rope))
                                {
                                    this.rope.SetTailLocation(DirectionVec.downRight, this.graph);
                                }
                            }

                            if (tailDidMove)
                            {
                                this.graph.ResetCurrentPlottedPoints(this.rope);
                                this.graph.PlotVisited(this.rope);
                                this.graph.DebugGraph();
                            }

                        }

                        // Console.WriteLine("----result after RIGHT operation");
                        this.graph.ResetCurrentPlottedPoints(this.rope);
                        this.graph.PlotVisited(this.rope);
                        this.graph.DebugGraph();
                        this.graph.DebugTailVisited();

                        previousOp = opName;
                        // return;
                    }
                    else if (opName == OpName.L)
                    {
                        for (int i = 0; i < moveAmount; i++)
                        {
                            // Console.WriteLine("---- moving LEFT! this amount {0}", moveAmount);
                            if (this.rope.SetHeadLocation(DirectionVec.left, this.graph))
                            {
                                this.graph.ResetCurrentPlottedPoints(this.rope);
                                this.graph.PlotVisited(this.rope);
                                this.graph.DebugGraph();
                            }
                            else
                            {
                                this.graph.ResetCurrentPlottedPoints(this.rope);
                                this.graph.PlotVisited(this.rope);
                                this.graph.DebugGraph();
                                continue;
                            }

                            this.graph.ResetCurrentPlottedPoints(this.rope);

                            IsAdjacentResult adjResult = this.graph.IsTailAdjacentToHead(this.rope);

                            if (!adjResult.isAdjacent && adjResult.diffVec is not null)
                            {
                                tailDidMove = true;

                                if (adjResult.diffVec.y == 2 && adjResult.diffVec.x == 0)
                                {
                                    this.rope.SetTailLocation(DirectionVec.left, this.graph);
                                }
                                else if (this.graph.isHeadDown(adjResult.diffVec.y == 2, this.rope))
                                {
                                    this.rope.SetTailLocation(DirectionVec.downLeft, this.graph);
                                }
                                else if (this.graph.isHeadUp(adjResult.diffVec.y == 2, this.rope))
                                {
                                    this.rope.SetTailLocation(DirectionVec.upLeft, this.graph);
                                }
                            }

                            if (tailDidMove)
                            {
                                this.graph.ResetCurrentPlottedPoints(this.rope);
                                this.graph.PlotVisited(this.rope);
                                this.graph.DebugGraph();
                            }
                        }

                        // Console.WriteLine("----result after LEFT operation");
                        this.graph.ResetCurrentPlottedPoints(this.rope);
                        this.graph.PlotVisited(this.rope);
                        this.graph.DebugGraph();

                        this.graph.DebugTailVisited();

                        previousOp = opName;
                        // return;

                    }
                    else if (opName == OpName.U)
                    {
                        for (int i = 0; i < moveAmount; i++)
                        {
                            // Console.WriteLine("---- moving UP! this amount {0}", moveAmount);
                            if (this.rope.SetHeadLocation(DirectionVec.up, this.graph))
                            {
                                this.graph.ResetCurrentPlottedPoints(this.rope);
                                this.graph.PlotVisited(this.rope);
                                this.graph.DebugGraph();
                            }
                            else
                            {
                                this.graph.ResetCurrentPlottedPoints(this.rope);
                                this.graph.PlotVisited(this.rope);
                                this.graph.DebugGraph();
                                continue;
                            }

                            this.graph.ResetCurrentPlottedPoints(this.rope);

                            IsAdjacentResult adjResult = this.graph.IsTailAdjacentToHead(this.rope);

                            if (!adjResult.isAdjacent && adjResult.diffVec is not null)
                            {
                                tailDidMove = true;

                                if (adjResult.diffVec.x == 2 && adjResult.diffVec.y == 0)
                                {
                                    this.rope.SetTailLocation(DirectionVec.up, this.graph);
                                }
                                else if (this.graph.isHeadLeft(adjResult.diffVec.x == 2, this.rope))
                                {
                                    this.rope.SetTailLocation(DirectionVec.upLeft, this.graph);
                                }
                                else if (this.graph.isHeadRight(adjResult.diffVec.x == 2, this.rope))
                                {
                                    this.rope.SetTailLocation(DirectionVec.upRight, this.graph);
                                }

                            }

                            if (tailDidMove)
                            {
                                this.graph.ResetCurrentPlottedPoints(this.rope);
                                this.graph.PlotVisited(this.rope);
                                this.graph.DebugGraph();
                            }
                        }

                        // Console.WriteLine("----result after UP operation");
                        this.graph.ResetCurrentPlottedPoints(this.rope);
                        this.graph.PlotVisited(this.rope);
                        this.graph.DebugGraph();

                        this.graph.DebugTailVisited();

                        previousOp = opName;
                        // return;
                    }
                    else if (opName == OpName.D)
                    {
                        for (int i = 0; i < moveAmount; i++)
                        {
                            // Console.WriteLine("---- moving DOWN! this amount {0}", moveAmount);
                            if (this.rope.SetHeadLocation(DirectionVec.down, this.graph))
                            {
                                this.graph.ResetCurrentPlottedPoints(this.rope);
                                this.graph.PlotVisited(this.rope);
                                this.graph.DebugGraph();
                            }
                            else
                            {
                                this.graph.ResetCurrentPlottedPoints(this.rope);
                                this.graph.PlotVisited(this.rope);
                                this.graph.DebugGraph();
                                continue;
                            }

                            this.graph.ResetCurrentPlottedPoints(this.rope);

                            IsAdjacentResult adjResult = this.graph.IsTailAdjacentToHead(this.rope);

                            if (!adjResult.isAdjacent && adjResult.diffVec is not null)
                            {
                                tailDidMove = true;
                                if (adjResult.diffVec.x == 2 && adjResult.diffVec.y == 0)
                                {
                                    this.rope.SetTailLocation(DirectionVec.down, this.graph);
                                }
                                else if (this.graph.isHeadRight(adjResult.diffVec.x == 2, this.rope))
                                {
                                    this.rope.SetTailLocation(DirectionVec.downRight, this.graph);
                                }
                                else if (this.graph.isHeadLeft(adjResult.diffVec.x == 2, this.rope))
                                {
                                    this.rope.SetTailLocation(DirectionVec.downLeft, this.graph);
                                }
                            }
                            if (tailDidMove)
                            {
                                this.graph.ResetCurrentPlottedPoints(this.rope);
                                this.graph.PlotVisited(this.rope);
                                this.graph.DebugGraph();
                            }
                        }

                        // Console.WriteLine("----result after DOWN operation");
                        this.graph.ResetCurrentPlottedPoints(this.rope);
                        this.graph.PlotVisited(this.rope);
                        this.graph.DebugGraph();
                        this.graph.DebugTailVisited();

                        previousOp = opName;
                        // return;
                    }
                }
            }
        }
        public double GetTailLocationsAmount()
        {
            double tailVisited = 0;
            foreach (List<string> line in this.graph.visited)
            {
                foreach (string chr in line)
                {
                    if (chr == "#") tailVisited++;
                }
            }
            return tailVisited;
        }

        public void GetInput(string fileName)
        {
            this.input = File.ReadAllText(fileName);
            this._lines = input.Split(Environment.NewLine);
        }
        public void PartOne()
        {
            this.ParseOps();
            this.InitGraph();
            this.InitPositionsOnGraph();

            this.graph.PlotVisited(this.rope);

            // Console.WriteLine("start");
            this.graph.DebugGraph();

            this.MoveOperations();

            this.graph.DebugTailVisited();

            Console.WriteLine("Part 1: {0}", this.GetTailLocationsAmount());
        }
        public void PartTwo()
        {
            Console.WriteLine("Part 2: {0}", "answer goes here");
        }
    }
}
