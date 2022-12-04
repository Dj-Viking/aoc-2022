namespace Day4
{
    class MainClass
    {
        public string input = "";
        public string[] _lines = new string[] { "" };
        public Dictionary<int, List<string>> _elfPairTable = new(); // list of pairs of elf cover ranges
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
            this._elfPairTable = new Dictionary<int, List<string>>();
        }
        public void GetInput(string fileName)
        {
            this.input = File.ReadAllText(fileName);
            this._lines = input.Split(Environment.NewLine);
        }
        public void CreateElfPairTable()
        {
            string[] lines = this._lines;
            List<string> elfPair = new();
            for (int i = 0; i < lines.Length; i++)
            {
                elfPair.Add(lines[i].Split(",")[0]);
                elfPair.Add(lines[i].Split(",")[1]);
                this._elfPairTable[i] = elfPair;
                elfPair = new List<string>();
            }
            // foreach (int pairIndex in this._elfPairTable.Keys)
            // {
            //     Console.WriteLine("current pairIndex [{0}]", string.Join(", ", this._elfPairTable[pairIndex]));
            // }
        }
        public List<string> CreateRangeListFromPair()
        {
            List<string> result = new();

            foreach (List<string> pair in this._elfPairTable.Values)
            {

            }

            return result;
        }
        public void PartOne()
        {
            this.CreateElfPairTable();
            string answer = "answer goes here";
            Console.WriteLine("Part 1: {0}", answer);
        }
        public void PartTwo()
        {
            string answer = "answer goes here";
            Console.WriteLine("Part 2: {0}", answer);
        }
    }
}
