namespace Day1
{
    class MainClass
    {
        public string input = "";
        public string[] _lines = new string[] { "" };
        public Dictionary<double, List<double>> _elves = new Dictionary<double, List<double>>();
        public Dictionary<double, double> _elfSums = new Dictionary<double, double>();
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
            this._elves = new Dictionary<double, List<double>>();
            this._elfSums = new Dictionary<double, double>();
        }
        private void ParseElfCollection()
        {
            string[] lines = this._lines;
            double elf = 0;
            List<double> foodList = new();

            for (int i = 0; i < lines.Length; i++)
            {
                if (!string.IsNullOrEmpty(lines[i]))
                {
                    foodList.Add(Int32.Parse((lines[i])));

                    // if got to last item add it to list and end the parsing!
                    if (i == lines.Length - 1)
                    {
                        this._elves[elf] = foodList;
                    }
                }
                else
                {
                    this._elves[elf] = foodList;
                    elf++;
                    foodList = new List<double>();
                }
            }
        }
        private void GatherElfCalorieSums()
        {
            double tempSum = 0;
            foreach (double elf in this._elves.Keys)
            {
                tempSum = this._elves[elf].Sum();
                this._elfSums.Add(elf, tempSum);
            }
        }
        private double GetHighestSum()
        {
            List<double> allSums = new();
            foreach (double elf in this._elfSums.Keys)
            {
                allSums.Add(this._elfSums[elf]);
            }
            return allSums.Max();
        }
        public void GetInput(string fileName)
        {
            this.input = File.ReadAllText(fileName);
            this._lines = input.Split("\n");
        }
        public void PartOne()
        {
            this.ParseElfCollection();
            this.GatherElfCalorieSums();
            Console.WriteLine("Part 1: {0}", this.GetHighestSum());
        }
        public void PartTwo()
        {
            Console.WriteLine("Part 2: ");
        }
    }
}
