namespace Day7
{
    class MainClass
    {
        public string input = "";
        public string[] _lines = new string[] { "" };
        public LinkedList<string> _linkedList = new();
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
        }
        public void CreateTree()
        {
            this._linkedList = new(this._lines);
        }
        public void DebugTree()
        {
            foreach (string item in this._linkedList)
            {
                Console.WriteLine("item in linked list {0}", item);
            }
        }
        public void GetInput(string fileName)
        {
            this.input = File.ReadAllText(fileName);
            this._lines = input.Split(Environment.NewLine);
        }
        public void PartOne()
        {
            this.CreateTree();
            Console.WriteLine("Part 1: {0}", "answer goes here");
        }
        public void PartTwo()
        {
            Console.WriteLine("Part 2: {0}", "answer goes here");
        }
    }
}
