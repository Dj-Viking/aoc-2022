namespace Day5
{
    class MainClass
    {
        public string input = "";
        public string[] _lines = new string[] { "" };
        public Dictionary<string, List<string>> _boxSections = new();
        public List<string> _instructions = new();
        public bool _isSample = false;
        public static void Main(string[] args)
        {
            new MainClass().Run(args);
        }
        public void Run(string[] args)
        {
            this.Init();
            this.GetInput(args[0]);
            this._isSample = args[0] == "sample.txt";
            this.PartOne();

            this.Init();
            this.GetInput(args[0]);
            this._isSample = args[0] == "sample.txt";
            this.PartTwo();
        }
        public void Init()
        {
            this.input = "";
            this._lines = new string[] { "" };
            this._boxSections = new Dictionary<string, List<string>>();
            this._instructions = new List<string>();
        }
        public void GetInput(string fileName)
        {
            this.input = File.ReadAllText(fileName);
            this._lines = input.Split(Environment.NewLine);
        }
        public void ParseBoxes()
        {
            int indexOfSectionNumbers = 0;
            for (int i = 0; i < this._lines.Length; i++)
            {
                if (string.IsNullOrEmpty(this._lines[i])) indexOfSectionNumbers = i - 1;
            }
            Console.WriteLine("what is index of empty line {0}", indexOfSectionNumbers);

            string[] sectionNumbers = this._lines[indexOfSectionNumbers].Split("_").ToList().Select(x => x.Trim()).ToArray();

            Console.WriteLine("section numbers [{0}]", string.Join(",", sectionNumbers));

            this.InitializeBoxDict(sectionNumbers);
            this.DebugBoxDict();


            for (int i = 0; i < (this._isSample ? indexOfSectionNumbers : indexOfSectionNumbers + 1); i++)
            {
                for (int j = 0; j < (this._isSample ? sectionNumbers.Length : sectionNumbers.Length - 1); j++)
                {// get the column and put the strings of that column in the dict section number it goes in
                    this._boxSections[(i + 1).ToString()].Add(this._lines[j][i].ToString());
                }
            }

            this.DebugBoxDict();


        }
        public void ParseInstructions()
        {
            int indexAfterEmptyLine = 0;
            for (int i = 0; i < this._lines.Length; i++)
            {
                if (string.IsNullOrEmpty(this._lines[i])) indexAfterEmptyLine = i + 1;
            }

            List<string> instructionLines = new();
            for (int i = indexAfterEmptyLine; i < this._lines.Length; i++)
            {
                instructionLines.Add(this._lines[i]);
            }
            Console.WriteLine("---- instruction lines \n{0}", string.Join("\n", instructionLines));


        }
        public void DebugBoxDict()
        {
            foreach (string key in this._boxSections.Keys)
            {
                Console.WriteLine("what is strlist for str {1} in box dict [{0}]", string.Join(", ", this._boxSections[key]), key);
            }
        }
        public void InitializeBoxDict(string[] nums)
        {
            foreach (string num in nums)
            {
                this._boxSections.Add(num, new List<string>());
            }
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
            // this.ParseInstructions();
            Console.WriteLine("Part 1: {0}", "answer goes here");
        }
        public void PartTwo()
        {
            string answer = "answer goes here";
            Console.WriteLine("Part 2: {0}", answer);
        }
    }
}
