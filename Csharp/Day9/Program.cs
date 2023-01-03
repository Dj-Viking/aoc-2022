using System.Runtime.CompilerServices;
namespace Day9
{
    class MainClass
    {
        public class Coordinates
        {
            public int x { get; set; }
            public int y { get; set; }
            public Coordinates(int _x, int _y)
            {
                this.x = _x;
                this.y = _y;
            }
        }
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
        public class Rope
        {
            public Head head = new Head();
            public Tail tail = new Tail();
            public class Head
            {

                public Coordinates Coords = new Coordinates(0, 0);
                public Head() { }
                public bool SetLocation(Vec2 vec2)
                {
                    int newCoordX = vec2.x += this.Coords.x;
                    int newCoordY = vec2.y += this.Coords.y;

                    if (newCoordX > MAX_DIM - 1 || newCoordY > MAX_DIM - 1) return false;
                    if (newCoordX < 0 || newCoordY < 0) return false;
                    Console.WriteLine("setting head location [{0}, {1}]", newCoordX, newCoordY);

                    this.Coords.x = vec2.x;
                    this.Coords.y = vec2.y;
                    return true;
                }
            }
            public class Tail
            {
                public Coordinates Coords = new Coordinates(0, 0);
                public bool[][] visited { get; set; } = new bool[][] { new bool[] { false } };
                public Tail() { }
                public bool SetLocation(Vec2 vec2)
                {
                    int newCoordX = vec2.x += this.Coords.x;
                    int newCoordY = vec2.y += this.Coords.y;

                    if (newCoordX > MAX_DIM - 1 || newCoordY > MAX_DIM - 1) return false;
                    if (newCoordX < 0 || newCoordY < 0) return false;
                    Console.WriteLine("setting tail location [{0}, {1}]", newCoordX, newCoordY);

                    this.Coords.x = vec2.x;
                    this.Coords.y = vec2.y;
                    this.visited[this.Coords.x][this.Coords.y] = true;
                    return true;
                }
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
            public string[][] grid { get; set; } = new string[][] { new string[] { "" } };
            public string[][] visited { get; set; } = new string[][] { new string[] { "" } };
            public Graph() { }
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
                for (int i = 0; i < rope.tail.visited.Length; i++)
                    for (int j = 0; j < rope.tail.visited[i].Length; j++)
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
                this.grid[START.x][START.y] = "s";
                this.grid[rope.tail.Coords.x][rope.tail.Coords.y] = "T";
                this.grid[rope.head.Coords.x][rope.head.Coords.y] = "H";
                this.TrackTailVisited(rope);
            }
            public void DebugGraph([CallerLineNumberAttribute] int lineNumber = 0)
            {
                Console.WriteLine($"----- debugging graph {lineNumber}");
                Console.Write("  ");
                for (int k = 0; k < this.grid.Length; k++)
                {
                    if (k < 10)
                    {
                        Console.Write($" {k} ");

                    }
                    else
                    {

                        Console.Write($"{k} ");
                    }
                }
                Console.WriteLine();
                for (int i = 0; i < this.grid.Length; i++)
                {
                    if (i < 10)
                    {
                        Console.Write($"{i} ");

                    }
                    else
                    {
                        Console.Write($"{i}");

                    }
                    for (int j = 0; j < this.grid[i].Length; j++)
                    {
                        Console.Write($" {this.grid[i][j]} ");
                    }
                    Console.WriteLine();
                }
            }
            public void DebugTailVisited([CallerLineNumberAttribute] int lineNumber = 0)
            {
                Console.WriteLine("----- debugging tail visited {0}", lineNumber);
                Console.Write("  ");
                for (int k = 0; k < this.visited.Length; k++)
                {
                    if (k < 10)
                    {
                        Console.Write($" {k} ");

                    }
                    else
                    {

                        Console.Write($"{k} ");
                    }
                }
                Console.WriteLine();
                for (int i = 0; i < this.visited.Length; i++)
                {
                    if (i < 10)
                    {
                        Console.Write($"{i} ");

                    }
                    else
                    {
                        Console.Write($"{i}");
                    }
                    for (int j = 0; j < this.visited[i].Length; j++)
                    {
                        Console.Write($" {this.visited[i][j]} ");
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

            this.graph.grid = strLists.Select(list => list.ToArray()).ToArray();
            this.graph.visited = strLists.Select(list => list.ToArray()).ToArray();
            this.rope.tail.visited = boolLists.Select(list => list.ToArray()).ToArray();
        }

        public void InitPositionsOnGraph()
        {
            START = new Start(MAX_DIM - 1, 0);
            this.rope.head.SetLocation(new Vec2(MAX_DIM - 1, 0));
            this.rope.tail.SetLocation(new Vec2(MAX_DIM - 1, 0));
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
                            this.graph.ResetCurrentPlottedPoints(this.rope);
                            Console.WriteLine("---- moving RIGHT! this amount {0}", moveAmount);
                            if (this.rope.head.SetLocation(DirectionVec.right))
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
                                    this.rope.tail.SetLocation(DirectionVec.right);
                                }
                                else if (this.graph.isHeadUp(adjResult.diffVec.y == 2, this.rope))
                                {
                                    this.rope.tail.SetLocation(DirectionVec.upRight);
                                }
                                else if (this.graph.isHeadDown(adjResult.diffVec.y == 2, this.rope))
                                {
                                    this.rope.tail.SetLocation(DirectionVec.downRight);
                                }
                            }

                            if (tailDidMove)
                            {
                                this.graph.ResetCurrentPlottedPoints(this.rope);
                                this.graph.PlotVisited(this.rope);
                                this.graph.DebugGraph();
                            }

                        }

                        Console.WriteLine("----result after RIGHT operation");
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
                            this.graph.ResetCurrentPlottedPoints(this.rope);
                            Console.WriteLine("---- moving LEFT! this amount {0}", moveAmount);
                            if (this.rope.head.SetLocation(DirectionVec.left))
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
                                    this.rope.tail.SetLocation(DirectionVec.left);
                                }
                                else if (this.graph.isHeadDown(adjResult.diffVec.y == 2, this.rope))
                                {
                                    this.rope.tail.SetLocation(DirectionVec.downLeft);
                                }
                                else if (this.graph.isHeadUp(adjResult.diffVec.y == 2, this.rope))
                                {
                                    this.rope.tail.SetLocation(DirectionVec.upLeft);
                                }
                            }

                            if (tailDidMove)
                            {
                                this.graph.ResetCurrentPlottedPoints(this.rope);
                                this.graph.PlotVisited(this.rope);
                                this.graph.DebugGraph();
                            }
                        }

                        Console.WriteLine("----result after LEFT operation");
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
                            this.graph.ResetCurrentPlottedPoints(this.rope);
                            Console.WriteLine("---- moving UP! this amount {0}", moveAmount);
                            if (this.rope.head.SetLocation(DirectionVec.up))
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
                                    this.rope.tail.SetLocation(DirectionVec.up);
                                }
                                else if (this.graph.isHeadLeft(adjResult.diffVec.x == 2, this.rope))
                                {
                                    this.rope.tail.SetLocation(DirectionVec.upLeft);
                                }
                                else if (this.graph.isHeadRight(adjResult.diffVec.x == 2, this.rope))
                                {
                                    this.rope.tail.SetLocation(DirectionVec.upRight);
                                }

                            }

                            if (tailDidMove)
                            {
                                this.graph.ResetCurrentPlottedPoints(this.rope);
                                this.graph.PlotVisited(this.rope);
                                this.graph.DebugGraph();
                            }
                        }

                        Console.WriteLine("----result after UP operation");
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
                            this.graph.ResetCurrentPlottedPoints(this.rope);
                            Console.WriteLine("---- moving DOWN! this amount {0}", moveAmount);
                            if (this.rope.head.SetLocation(DirectionVec.down))
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
                                    this.rope.tail.SetLocation(DirectionVec.down);
                                }
                                else if (this.graph.isHeadRight(adjResult.diffVec.x == 2, this.rope))
                                {
                                    this.rope.tail.SetLocation(DirectionVec.downRight);
                                }
                                else if (this.graph.isHeadLeft(adjResult.diffVec.x == 2, this.rope))
                                {
                                    this.rope.tail.SetLocation(DirectionVec.downLeft);
                                }
                            }
                            if (tailDidMove)
                            {
                                this.graph.ResetCurrentPlottedPoints(this.rope);
                                this.graph.PlotVisited(this.rope);
                                this.graph.DebugGraph();
                            }
                        }

                        Console.WriteLine("----result after DOWN operation");
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
            foreach (string[] line in this.graph.visited)
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

            Console.WriteLine("start");
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
