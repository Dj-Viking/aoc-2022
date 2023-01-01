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
            public Graph() { }
            public void PlotVisited(Start start, Rope rope)
            {
                this.grid[start.x][start.y] = "s";
                this.grid[rope.tail.Coords.x][rope.tail.Coords.y] = "T";
                this.grid[rope.head.Coords.x][rope.head.Coords.y] = "H";
            }
            public void DebugGraph()
            {
                Console.WriteLine("----- debugging graph");
                foreach (string[] line in this.grid)
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
        public Start start = new Start(0, 0);
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
            this.start = new Start(0, 0);
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

            List<List<string>> lists = new();
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
                lists.Add(tempList);
                boolLists.Add(tempList2);
            }

            this.graph.grid = lists.Select(list => list.ToArray()).ToArray();
            this.rope.tail.visited = boolLists.Select(list => list.ToArray()).ToArray();
        }

        public void InitPositionsOnGraph()
        {
            this.start = new Start(this.MAX_DIM - 1, 0);
            this.rope.head.SetLocation(this.MAX_DIM - 1, 0);
            this.rope.tail.SetLocation(this.MAX_DIM - 1, 0);
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
            this.graph.PlotVisited(this.start, this.rope);
            this.graph.DebugGraph();
            Console.WriteLine("Part 1: {0}", "answer goes here");
        }
        public void PartTwo()
        {
            Console.WriteLine("Part 2: {0}", "answer goes here");
        }
    }
}
