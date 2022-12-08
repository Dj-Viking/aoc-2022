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
        public bool _isSample = false;
        public static void Main(string[] args)
        {
            new MainClass().Run(args);
        }
        public void Run(string[] args)
        {
            this.Init();
            this.GetInput(args[0]);
            this._isSample = args[0] == "sample";
            this.PartOne();

            this.Init();
            this.GetInput(args[0]);
            this._isSample = args[0] == "sample";
            this.PartTwo();
        }
        public void Init()
        {
            this.input = "";
            this._lines = new string[] { "" };
            this._elfPairTable = new Dictionary<int, List<string>>();
            this._isFullyContainedCounter = 0;
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
        }
        public List<bool[]> CreateElfPairRangeList(bool isSample, List<string> pair)
        {
            List<bool[]> rangeList = new();

            int array_size = isSample ? this._fullRange1 : this._fullRange2;

            bool[] tempRangeAlloc = new bool[array_size];

            // get min and max of pair and create bool range of spots they occupy
            int listMin1 = Int32.Parse(pair[0].Split("-")[0]);
            int listMax1 = Int32.Parse(pair[0].Split("-")[1]);
            int listMin2 = Int32.Parse(pair[1].Split("-")[0]);
            int listMax2 = Int32.Parse(pair[1].Split("-")[1]);

            for (int j = 0; j < tempRangeAlloc.Length; j++)
            {
                if ((j + 1) >= listMin1 && (j + 1) <= listMax1)
                {
                    tempRangeAlloc[j] = true;
                }
            }

            rangeList.Add(tempRangeAlloc);

            tempRangeAlloc = new bool[array_size];

            for (int j = 0; j < tempRangeAlloc.Length; j++)
            {
                if ((j + 1) >= listMin2 && (j + 1) <= listMax2)
                {
                    tempRangeAlloc[j] = true;
                }
            }

            rangeList.Add(tempRangeAlloc);

            return rangeList;

        }
        public void PartOne()
        {
            this.CreateElfPairTable();

            foreach (List<string> elf in this._elfPairTable.Values)
            {
                List<bool[]> elfPairRangeList = this.CreateElfPairRangeList(this._isSample, elf);
                int range_size = elfPairRangeList[0].Length;

                int bothTrueCounter = 0;

                for (int i = 0; i < range_size; i++)
                {
                    if (elfPairRangeList[1][i] == true &&
                        elfPairRangeList[0][i] == true)
                    {
                        bothTrueCounter++;
                    }
                }

                if (bothTrueCounter == elfPairRangeList[1].ToList().FindAll(x => x == true).Count() ||
                    bothTrueCounter == elfPairRangeList[0].ToList().FindAll(x => x == true).Count())
                {
                    this._isFullyContainedCounter++;
                }

            }
            Console.WriteLine("Part 1: {0}", this._isFullyContainedCounter);
        }
        public void PartTwo()
        {
            string answer = "answer goes here";
            Console.WriteLine("Part 2: {0}", answer);
        }
    }
}
