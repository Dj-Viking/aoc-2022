namespace Day3
{
    class MainClass
    {
        public string input = "";
        public string[] _lines = new string[] { "" };
        public RuckSackList _ruckSackList = new();
        public class RuckSack
        {
            public List<char> _comp_1;
            public List<char> _comp_2;
            public RuckSack()
            {
                this._comp_1 = new List<char>();
                this._comp_2 = new List<char>();
            }
        }
        public class PriorityTable : Dictionary<char, int>
        {
            public PriorityTable()
            {
                for (int i = 0; i < 52; i++)
                {
                    // this[i + 'A'] = i
                }
            }
        }
        public class RuckSackList : List<RuckSack>
        {

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
            this._ruckSackList = new RuckSackList();
        }
        public void GetInput(string fileName)
        {
            this.input = File.ReadAllText(fileName);
            this._lines = input.Split(Environment.NewLine);
        }
        public void ParseRuckSackList()
        {
            string[] lines = this._lines;
            for (int i = 0; i < lines.Length; i++)
            {
                RuckSack ruckSack = new();
                int halfLengthOfLine = lines[i].Length / 2;

                for (int j = 0; j < halfLengthOfLine; j++)
                {
                    ruckSack._comp_1.Add(lines[i][j]);
                }

                for (int j = halfLengthOfLine; j < lines[i].Length; j++)
                {
                    ruckSack._comp_2.Add(lines[i][j]);
                }
                this._ruckSackList.Add(ruckSack);
            }
        }
        public void PartOne()
        {
            this.ParseRuckSackList();
            foreach (RuckSack sack in this._ruckSackList)
            {
                Console.WriteLine("rucksack comp 1 [{0}] comp 2 [{1}]", string.Join(", ", sack._comp_1), string.Join(", ", sack._comp_2));
            }
            Console.WriteLine("Part 1: ");
        }
        public void PartTwo()
        {
            Console.WriteLine("Part 2: ");
        }
    }
}

// first half of characters in rucksack represent items in the first compartment

// lower case and uppercase are different types of items

// each rucksack always has same number of items in both compartments
