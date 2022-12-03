namespace Day3
{
    class MainClass
    {
        public string input = "";
        public string[] _lines = new string[] { "" };
        public RuckSackList _ruckSackList = new();
        public PriorityTable _priorityTable = new();
        public List<char> _priorityList = new();
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
                int value_inc_for_a_to_z = 1;
                int value_inc_for_A_to_Z = 27;
                int startingAt_a = 32 + 'A';
                int startingAt_A = 0 + 'A';
                int alphabet_count = 26;

                for (int i = 0; i < 58; i++)
                {
                    if (i >= 26 && i <= 31) continue; // skip ascii chars between Z and a in the ascii table
                    this[(char)(i + 'A')] = 0;
                }

                for (int i = 0; i < alphabet_count; i++)
                {
                    this[(char)(startingAt_a + i)] = value_inc_for_a_to_z++;
                    this[(char)(startingAt_A + i)] = value_inc_for_A_to_Z++;
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
            this._priorityTable = new PriorityTable();
            this._priorityList = new List<char>();
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
        public void GetPriorityList()
        {
            // which letter appears in both compartments of each sack has priority add to priority list
            // sort each rs compartment?
            foreach (RuckSack rs in this._ruckSackList)
            {
                for (int i = 0; i < rs._comp_1.Count(); i++)
                {
                    char currentChar = rs._comp_1[i];
                    int amountOfCurrentCharInComp1 = rs._comp_1.FindAll(x => x == currentChar).Count();
                    int amountOfCurrentCharInComp2 = rs._comp_2.FindAll(x => x == currentChar).Count();

                    if (amountOfCurrentCharInComp1 >= 1 && amountOfCurrentCharInComp2 >= 1)
                    {
                        this._priorityList.Add(currentChar);
                        break;

                    }
                }
            }

        }
        public void DebugRuckSacks()
        {
            Console.WriteLine("--- debug rucksacks");
            foreach (RuckSack sack in this._ruckSackList)
            {
                Console.WriteLine("---");
                Console.WriteLine("rucksack comp 1 [{0}] \nrucksack comp 2 [{1}]", string.Join(", ", sack._comp_1), string.Join(", ", sack._comp_2));
            }
        }
        public double CalculatePriorityValueSum()
        {
            double result = 0;
            for (int i = 0; i < this._priorityList.Count(); i++)
            {
                result += this._priorityTable[this._priorityList[i]];
            }
            return result;
        }
        public void PartOne()
        {
            this.ParseRuckSackList();
            this.GetPriorityList();
            Console.WriteLine("Part 1: {0}", this.CalculatePriorityValueSum());
        }
        public void PartTwo()
        {
            string answer = "answer goes here";
            Console.WriteLine("Part 2: {0}", answer);
        }
    }
}

// first half of characters in rucksack represent items in the first compartment

// lower case and uppercase are different types of items

// each rucksack always has same number of items in both compartments
