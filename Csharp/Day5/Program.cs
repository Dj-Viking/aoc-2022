namespace Day5
{
    class MainClass
    {
        public string input = "";
        public string[] _lines = new string[] { "" };
        public Dictionary<int, List<string>> _boxSections = new();
        public List<string> _instructions = new();
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
            this._boxSections = new Dictionary<int, List<string>>();
            this._instructions = new List<string>();
        }
        public void GetInput(string fileName)
        {
            this.input = File.ReadAllText(fileName);
            this._lines = input.Split(Environment.NewLine);
        }
        public void ParseBoxes()
        {
            this.DebugLines();
            string[] sectionNumbers = this._lines[3].Split("_").ToList().Select(x => x.Trim()).ToArray();

            Console.WriteLine("section numbers [{0}]", string.Join(",", sectionNumbers));

        }
        public void DebugLines()
        {
            for (int i = 0; i < this._lines.Length; i++)
            {
                Console.WriteLine("debug line {0}", this._lines[i]);
            }
        }
        public void PartOne()
        {
            this.ParseBoxes();
            Console.WriteLine("Part 1: {0}", "answer goes here");
        }
        public void PartTwo()
        {
            string answer = "answer goes here";
            Console.WriteLine("Part 2: {0}", answer);
        }
    }
}
