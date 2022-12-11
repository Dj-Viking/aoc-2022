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
            string[] sectionNumbers = this._lines[indexOfSectionNumbers].Split("_").ToList().Select(x => x.Trim()).ToArray();

            this.InitializeBoxDict(sectionNumbers);

            for (int i = 0; i < (this._isSample ? indexOfSectionNumbers : indexOfSectionNumbers + 1); i++)
            {
                for (int j = 0; j < (this._isSample ? sectionNumbers.Length : sectionNumbers.Length - 1); j++)
                {// get the column and put the strings of that column in the dict section number it goes in
                    this._boxSections[(i + 1).ToString()].Add(this._lines[j][i].ToString());
                }
            }

            // remove dollar signs used as placeholders
            foreach (string key in this._boxSections.Keys)
            {
                this._boxSections[key] = this._boxSections[key].FindAll(x => x != "$");
            }


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
            this._instructions = instructionLines;
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
        public void InterpretOp(string op)
        {
            string[] opSplit = op.Split(" ");

            string amountToMove = "";
            string sourceToMove = "";
            string destToMove = "";

            for (int i = 0; i < opSplit.Length; i++)
            {
                if (opSplit[i] == "move")
                {
                    amountToMove = opSplit[i + 1];
                }
                if (opSplit[i] == "from")
                {
                    sourceToMove = opSplit[i + 1];
                }
                if (opSplit[i] == "to")
                {
                    destToMove = opSplit[i + 1];
                }
            }

            this.Move(amountToMove, sourceToMove, destToMove);
        }
        public void Move(string amount, string source, string dest)
        {
            int take = Int32.Parse(amount);
            List<string> thingsToMove = new();

            for (int i = 0; i < take; i++)
            {
                thingsToMove.Add(this._boxSections[source][i]);
            }

            for (int i = 0; i < thingsToMove.Count(); i++)
            {
                string popped = this._boxSections[source][0];
                this._boxSections[source].RemoveAt(0);
                this._boxSections[dest] = this._boxSections[dest].Prepend(thingsToMove[i]).ToList();
            }

        }
        public bool HasEmptySlots(List<string> boxSection)
        {
            int emptySlots = boxSection.FindAll(x => x == "$").Count();
            if (emptySlots > 0) return true;
            return false;
        }
        public void PartOne()
        {
            this.ParseBoxes();
            this.ParseInstructions();

            for (int i = 0; i < this._instructions.Count(); i++)
            {
                this.InterpretOp(this._instructions[i]);
            }
            List<string> answerList = new();
            foreach (string key in this._boxSections.Keys)
            {
                answerList = answerList.Prepend(this._boxSections[key][0]).ToList();
            }
            Console.WriteLine("Part 1: {0}", string.Join("", answerList));
        }
        public void PartTwo()
        {
            string answer = "answer goes here";
            Console.WriteLine("Part 2: {0}", answer);
        }
    }
}
