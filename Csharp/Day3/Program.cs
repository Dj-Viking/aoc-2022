namespace Day3
{
    class MainClass
    {
        public string input = "";
        public string[] _lines = new string[] { "" };
        public DividedRuckSackList _ruckSackList = new();
        public PriorityTable _priorityTable = new();
        public List<char> _priorityList = new();
        public Dictionary<int, List<string>> _groupedSackList = new();
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
        public class DividedRuckSackList : List<RuckSack> { }
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
            this._ruckSackList = new DividedRuckSackList();
            this._priorityTable = new PriorityTable();
            this._priorityList = new List<char>();
            this._groupedSackList = new Dictionary<int, List<string>>();
        }
        public void GetInput(string fileName)
        {
            this.input = File.ReadAllText(fileName);
            this._lines = input.Split(Environment.NewLine);
        }
        public DividedRuckSackList ParseRuckSackList()
        {
            string[] lines = this._lines;
            DividedRuckSackList ruckSackList = new();

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
                ruckSackList.Add(ruckSack);
            }

            return ruckSackList;
        }
        public void ParseElfGroups()
        {
            // parse a group of three elves holding a rucksack (not divided in half) from the main list of rucksacks
            string[] lines = this._lines;
            List<string> tempList = new();
            for (int i = 0; i < lines.Length; i++)
            {
                tempList.Add(lines[i]);
                if (tempList.Count() == 3)
                {
                    int createdGroupNum = i == 2 ? 1 : i - 3;
                    this._groupedSackList[createdGroupNum] = tempList;
                    tempList = new List<string>();
                }
            }
        }
        public List<char> GetPriorityList2()
        {

            List<char> priorityList = new();
            foreach (List<string> elfGroup in this._groupedSackList.Values)
            {
                // just iterate through one string and then check all other elves what they have in common with the first sack
                for (int j = 0; j < elfGroup[0].Length; j++)
                {
                    char currentChar = elfGroup[0][j];
                    int amountInOne = elfGroup[0].ToCharArray().ToList().FindAll(x => x == currentChar).Count();
                    int amountInTwo = elfGroup[1].ToCharArray().ToList().FindAll(x => x == currentChar).Count();
                    int amountInThree = elfGroup[2].ToCharArray().ToList().FindAll(x => x == currentChar).Count();

                    if (amountInOne >= 1 && amountInTwo >= 1 && amountInThree >= 1)
                    {
                        priorityList.Add(currentChar);
                        break;
                    }
                }
            }
            return priorityList;
        }
        public List<char> GetPriorityList1(DividedRuckSackList rsList)
        {
            // which letter appears in both compartments of each sack has priority add to priority list

            List<char> priorityList = new();

            foreach (RuckSack rs in rsList)
            {
                for (int i = 0; i < rs._comp_1.Count(); i++)
                {
                    char currentChar = rs._comp_1[i];
                    int amountOfCurrentCharInComp1 = rs._comp_1.FindAll(x => x == currentChar).Count();
                    int amountOfCurrentCharInComp2 = rs._comp_2.FindAll(x => x == currentChar).Count();

                    if (amountOfCurrentCharInComp1 >= 1 && amountOfCurrentCharInComp2 >= 1)
                    {
                        priorityList.Add(currentChar);
                        break;
                    }
                }
            }

            return priorityList;

        }
        public void DebugGroupedSacks()
        {
            Console.WriteLine("--- debug grouped sacks");
            foreach (int key in this._groupedSackList.Keys)
            {
                Console.WriteLine("for group [{0}] \nhas these lists:", key);
                for (int i = 0; i < this._groupedSackList[key].Count(); i++)
                {
                    Console.Write("[{0}]\n", string.Join(", ", this._groupedSackList[key][i]));
                }
            }
        }
        public void DebugRuckSacks()
        {
            Console.WriteLine("--- debug rucksacks");
            foreach (RuckSack rs in this._ruckSackList)
            {
                Console.WriteLine("---");
                Console.WriteLine("rucksack comp 1 [{0}] \nrucksack comp 2 [{1}]", string.Join(", ", rs._comp_1), string.Join(", ", rs._comp_2));
            }
        }
        public double CalculatePriorityValueSum(List<char> priorityList)
        {
            double result = 0;
            for (int i = 0; i < priorityList.Count(); i++)
            {
                result += this._priorityTable[priorityList[i]];
            }
            return result;
        }
        public void PartOne()
        {
            this._ruckSackList = this.ParseRuckSackList();
            this._priorityList = this.GetPriorityList1(this._ruckSackList);
            Console.WriteLine("Part 1: {0}", this.CalculatePriorityValueSum(this._priorityList));
        }
        public void PartTwo()
        {
            this.ParseElfGroups();
            this._priorityList = this.GetPriorityList2();
            Console.WriteLine("Part 2: {0}", this.CalculatePriorityValueSum(this._priorityList));
        }
    }
}


// part 1
// first half of characters in rucksack represent items in the first compartment

// lower case and uppercase are different types of items

// each rucksack always has same number of items in both compartments

// part 2
// every elf carries a badge that identifies their group

// each badge is the only item type carried by all three elves in the group
// for example
/** 
 elfgroup A {
    A83jf0jfsjfdjfksjdfs
    jfjfiejiewowppApipoqie
    ieriuwpeirAkdjfkdjkfdfj
 }

*/

// at most - two of the elves will be carrying any other item type

// find the badge which is common between all three elves in the separate group of three elves 