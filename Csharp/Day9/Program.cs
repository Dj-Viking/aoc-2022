namespace Day9
{
    class MainClass
    {
        public string input = "";
        public string[] _lines = new string[] { "" };
        public List<Dictionary<string, int>> OpMapList = new();
        public string[][] Graph = new string[][] { new string[] { "" } };
        public class OpName
        {
            public static readonly string R = "R";
            public static readonly string L = "L";
            public static readonly string U = "U";
            public static readonly string D = "D";
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
            int max_dim = 0;
            List<int> dimensionList = new();
            foreach (Dictionary<string, int> opObj in this.OpMapList)
            {
                foreach (string opName in opObj.Keys)
                {
                    dimensionList.Add(opObj[opName]);
                }
            }

            max_dim = dimensionList.Max() + 1;

            List<List<string>> lists = new();

            for (int i = 0; i < max_dim; i++)
            {
                List<string> tempList = new();
                for (int j = 0; j < max_dim; j++)
                {
                    tempList.Add(".");
                }
                lists.Add(tempList);
            }

            this.Graph = lists.Select(x => x.ToArray()).ToArray();
        }
        public void DebugGraph()
        {
            Console.WriteLine("----- debugging graph");
            foreach (string[] line in this.Graph)
            {
                foreach (string str in line)
                {
                    Console.Write($" {str} ");
                }
                Console.WriteLine();
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
            this.DebugGraph();
            Console.WriteLine("Part 1: {0}", "answer goes here");
        }
        public void PartTwo()
        {
            Console.WriteLine("Part 2: {0}", "answer goes here");
        }
    }
}
