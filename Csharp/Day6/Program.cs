namespace Day6
{
    class MainClass
    {
        public string input = "";
        public string[] _lines = new string[] { "" };
        public double _amountToProcess1 = 0;
        public double _amountToProcess2 = 0;
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
            this._amountToProcess1 = 0;
            this._amountToProcess2 = 0;
        }
        public void GetInput(string fileName)
        {
            this.input = File.ReadAllText(fileName);
            this._lines = input.Split(Environment.NewLine);
        }
        public void ReadDataStream1()
        {
            char[] splitCharStream = this._lines[0].ToCharArray();
            List<char> packetStorage = new();

            for (int i = 1; i < splitCharStream.Length - 1; i++)
            {
                char currentChar = splitCharStream[i];
                char previousChar = splitCharStream[i - 1];
                packetStorage.Add(previousChar);
                if (packetStorage.Count() == 4)
                {
                    char fourthChar = packetStorage[3];
                    int charCount = packetStorage.FindAll(x => x == fourthChar).Count();
                    if (charCount > 1) continue;
                }
                else if (packetStorage.Count() > 4)
                {
                    List<char> recentFour = new();
                    for (int j = packetStorage.Count() - 1; j > packetStorage.Count() - 5; j--)
                    {
                        recentFour.Add(packetStorage[j]);
                    }

                    bool isUnique = recentFour.Distinct().Count() == recentFour.Count();
                    if (isUnique)
                    {
                        this._amountToProcess1 = packetStorage.Count();
                        return;
                    }

                }

            }
        }
        public void ReadDataStream2()
        {
            char[] splitCharStream = this._lines[0].ToCharArray();
            List<char> packetStorage = new();

            for (int i = 1; i < splitCharStream.Length - 1; i++)
            {
                char currentChar = splitCharStream[i];
                char previousChar = splitCharStream[i - 1];
                packetStorage.Add(previousChar);
                if (packetStorage.Count() == 14)
                {
                    char fourthChar = packetStorage[3];
                    int charCount = packetStorage.FindAll(x => x == fourthChar).Count();
                    if (charCount > 1) continue;
                }
                else if (packetStorage.Count() > 14)
                {
                    List<char> recentFourteen = new();
                    for (int j = packetStorage.Count() - 1; j > packetStorage.Count() - 15; j--)
                    {
                        recentFourteen.Add(packetStorage[j]);
                    }

                    bool isUnique = recentFourteen.Distinct().Count() == recentFourteen.Count();
                    if (isUnique)
                    {
                        this._amountToProcess2 = packetStorage.Count();
                        return;
                    }

                }

            }
        }
        public void PartOne()
        {
            this.ReadDataStream1();
            Console.WriteLine("Part 1: {0}", this._amountToProcess1);
        }
        public void PartTwo()
        {
            this.ReadDataStream2();
            Console.WriteLine("Part 2: {0}", this._amountToProcess2);
        }
    }
}


// identify the first position where the four most recently received characters were all different.

// the start of a packet is indicated by a sequence of four characters that are all different.

// 