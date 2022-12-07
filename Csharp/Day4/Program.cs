namespace Day4
{
    class MainClass
    {
        public string input = "";
        public string[] _lines = new string[] { "" };
        public Dictionary<int, List<string>> _elfPairTable = new(); // list of pairs of elf cover ranges
        public int _fullRange1 = 9;
        public int _fullRange2 = 99;
        public double _isFullyContainedCounter = 0;
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
            this._isFullyContainedCounter = 0;
            

            Console.WriteLine("what is the bool array {0}", string.Join(", ", _fullRange1));
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
            foreach (int pairIndex in this._elfPairTable.Keys)
            {
                Console.WriteLine("current pairIndex [{0}]", string.Join(", ", this._elfPairTable[pairIndex]));
            }
        }
        public List<bool[]> CreateElfPairRangeList(bool isSample, List<string> elf)
        {
            List<bool[]> rangeList = new();
            int array_size = isSample ? this._fullRange1 : this._fullRange2;
            bool[] tempRangeAlloc = new bool[array_size];

            // get min and max of pair and create bool range of spots they occupy
            for (int i = 0; i < elf.Count(); i++) {
                Console.WriteLine("what is pair here {0}", elf[i]);
                int listMin = Int32.Parse(elf[i].Split("-")[0]);
                int listMax = Int32.Parse(elf[i].Split("-")[1]);
                for (int j = 0; j < tempRangeAlloc.Length; j++) {
                    if ((j + 1) >= listMin && (j + 1) <= listMax) {
                        tempRangeAlloc[j] = true;
                    }
                }
                Console.WriteLine("what is temprange alloc of bools [{0}]\n", string.Join(", ", tempRangeAlloc));
                tempRangeAlloc = new bool[array_size];
                return new List<bool[]>();
            }

            return rangeList;
        }
        public void PartOne()
        {
            this.CreateElfPairTable();
            foreach (List<string> elf in this._elfPairTable.Values) 
            {
                List<bool[]> elfPairRangeList = this.CreateElfPairRangeList(true, elf);
            }
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
