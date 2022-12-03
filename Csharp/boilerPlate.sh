cat << EOF > Program.cs
namespace $1
{
    class MainClass
    {
        public string input = "";
        public string[] _lines = new string[] { "" };
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
        public void GetInput(string fileName)
        {
            this.input = File.ReadAllText(fileName);
            this._lines = input.Split(Environment.NewLine);
        }
        public void PartOne()
        {
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
EOF