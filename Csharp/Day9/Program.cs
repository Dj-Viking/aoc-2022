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
        public string input = "";
        public string[] _lines = new string[] { "" };
        public List<Dictionary<string, int>> OpMapList = new();
        Graph graph = new Graph();
        public class Graph
        {
            public string[][] grid { get; set; } = new string[][] { new string[] { "" } };
            public string[][] visited { get; set; } = new string[][] { new string[] { "" } };
            public Graph() { }
            public bool TailShouldMove(Rope rope, bool isLastStep)
            {
                Console.WriteLine("was last move? {0}");
                int currentRopeHeadX = rope.head.Coords.x;
                int currentRopeHeadY = rope.head.Coords.y;
                int currentRopeTailX = rope.tail.Coords.x;
                int currentRopeTailY = rope.tail.Coords.y;

                return true;

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
                this.grid[START.x][START.y] = "s";
                this.grid[rope.tail.Coords.x][rope.tail.Coords.y] = "T";
                this.grid[rope.head.Coords.x][rope.head.Coords.y] = "H";
                this.TrackTailVisited(rope);
            }
            public void DebugGraph(string lineNumber)
            {
                Console.WriteLine($"----- debugging graph {lineNumber}");
                foreach (string[] line in this.grid)
                {
                    foreach (string str in line)
                    {
                        Console.Write($" {str} ");
                    }
                    Console.WriteLine();
                }
            }
            public void DebugTailVisited()
            {
                Console.WriteLine("----- debugging tail visited");
                foreach (string[] line in this.visited)
                {
                    foreach (string str in line)
                    {
                        Console.Write($" {str} ");
                    }
                    Console.WriteLine();
                }
            }
        }
        public int MAX_DIM = 0;
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
                public void SetLocation(int x, int y)
                {
                    this.Coords.x = x;
                    this.Coords.y = y;
                }
            }
            public class Tail
            {
                public Coordinates Coords = new Coordinates(0, 0);
                public bool[][] visited { get; set; } = new bool[][] { new bool[] { false } };
                public Tail() { }
                public void SetLocation(int x, int y)
                {
                    this.Coords.x = x;
                    this.Coords.y = y;
                    this.visited[x][y] = true;
                }
            }

        }
        public Rope rope = new Rope();
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

            this.MAX_DIM = dimensionList.Max() + 1;

            List<List<string>> strLists = new();
            List<List<bool>> boolLists = new();

            for (int i = 0; i < this.MAX_DIM; i++)
            {
                List<string> tempList = new();
                List<bool> tempList2 = new();
                for (int j = 0; j < this.MAX_DIM; j++)
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
            START = new Start(this.MAX_DIM - 1, 0);
            this.rope.head.SetLocation(this.MAX_DIM - 1, 0);
            this.rope.tail.SetLocation(this.MAX_DIM - 1, 0);
        }
        public void MoveOperations()
        {
            string previousOp = "";
            foreach (Dictionary<string, int> opObj in this.OpMapList)
            {
                foreach (string opName in opObj.Keys)
                {
                    int moveAmount = opObj[opName];
                    if (opName == OpName.R)
                    {

                        int newYCoordinate = moveAmount + this.rope.head.Coords.y;
                        Console.WriteLine("what is move amount{0}", newYCoordinate);
                        int beginHeadY = this.rope.head.Coords.y;

                        for (int i = beginHeadY; i < newYCoordinate; i++)
                        {
                            // start moving head iteratively ahead of tail
                            this.rope.head.SetLocation(this.rope.head.Coords.x, i);
                            // move tail lagging behind by one
                            int moveTail = i == 0 ? 0 : i - 1;
                            if (i == 0) continue;
                            this.rope.tail.SetLocation(this.rope.tail.Coords.x, moveTail);

                            this.graph.PlotVisited(this.rope);

                            Console.WriteLine("what direction should be right {0}", opName);
                            this.graph.DebugGraph("223");

                            this.graph.ResetCurrentPlottedPoints(this.rope);
                        }


                        this.graph.PlotVisited(this.rope);
                        Console.WriteLine("----result after right operation");
                        this.graph.DebugGraph("229");
                        this.graph.ResetCurrentPlottedPoints(this.rope);

                        previousOp = opName;
                        // return;
                    }
                    else if (opName == OpName.L)
                    {

                    }
                    else if (opName == OpName.U)
                    {
                        this.graph.PlotVisited(this.rope);
                        int newXCoordinate = moveAmount - this.rope.head.Coords.x;
                        int beginHeadX = this.rope.head.Coords.x;
                        Console.WriteLine("what is direction and move amount {0} {1} - begin head x {2}", opName, moveAmount, beginHeadX);

                        for (int i = newXCoordinate; i >= beginHeadX; i--)
                        {
                            // start moving head iteratively ahead of tail
                            this.rope.head.SetLocation(i, this.rope.head.Coords.y);
                            // move tail lagging behind by one
                            int moveTail = i == 0 ? 0 : i - 1;
                            Console.WriteLine("what is outof bounds {0}", moveTail);
                            // if (i == 0) continue;
                            this.rope.tail.SetLocation(moveTail, this.rope.tail.Coords.y);


                            Console.WriteLine("what direction should be up {0}", opName);
                            this.graph.DebugGraph("257");
                            this.graph.DebugGraph("257");

                            this.graph.ResetCurrentPlottedPoints(this.rope);
                        }

                        this.graph.DebugGraph("263");

                        previousOp = opName;
                        return;
                    }
                    else if (opName == OpName.D)
                    {

                    }
                }
            }
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

            this.MoveOperations();

            this.graph.DebugTailVisited();

            Console.WriteLine("Part 1: {0}", "answer goes here");
        }
        public void PartTwo()
        {
            Console.WriteLine("Part 2: {0}", "answer goes here");
        }
    }
}
