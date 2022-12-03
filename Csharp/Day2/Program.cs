namespace Day2
{
    class MainClass
    {
        public string input = "";
        public string[] _lines = new string[] { "" };
        public ElfOpponent _elf = new ElfOpponent();
        public DJViking _djViking = new DJViking();
        public Dictionary<char, string> _debugTable = new();
        public class Player
        {
            public Dictionary<char, int> _choices = new();
            public double _score = 0;
            public void WonGame(char choice) { this._score += this._choices[choice] + 6; }
            public void DrawGame(char choice) { this._score += this._choices[choice] + 3; }
            public void LostGame(char choice) { this._score += this._choices[choice] + 0; }
        }
        public class ElfOpponent : Player
        {
            public ElfOpponent()
            {
                this._score = 0;
                this._choices['A'] = 1;
                this._choices['B'] = 2;
                this._choices['C'] = 3;
            }
        }
        public class DJViking : Player
        {
            public DJViking()
            {
                this._score = 0;
                this._choices['X'] = 1;
                this._choices['Y'] = 2;
                this._choices['Z'] = 3;
            }
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
            this._elf = new ElfOpponent();
            this._djViking = new DJViking();

            this._debugTable['A'] = "rock";
            this._debugTable['B'] = "paper";
            this._debugTable['C'] = "scissors";
            this._debugTable['X'] = "rock";
            this._debugTable['Y'] = "paper";
            this._debugTable['Z'] = "scissors";

        }
        public void GetInput(string fileName)
        {
            this.input = File.ReadAllText(fileName);
            this._lines = input.Split(Environment.NewLine);
        }
        public bool DrawCase(char e, char v)
        {
            return (e == 'A' && v == 'X') || (e == 'B' && v == 'Y') || (e == 'C' && v == 'Z');
        }
        public bool ElfWonVikingLost(char e, char v)
        {
            return (e == 'B' && v == 'X') || (e == 'A' && v == 'Z') || e == 'C' && v == 'Y';
        }
        public bool VikingWonElfLost(char e, char v)
        {
            return (v == 'X' && e == 'C') || (v == 'Y' && e == 'A') || (v == 'Z' && e == 'B');
        }
        public void Step(char elfChoice, char vikingChoice)
        {
            if (this.DrawCase(elfChoice, vikingChoice))
            {
                this._djViking.DrawGame(vikingChoice);
                this._elf.DrawGame(elfChoice);
            }
            if (this.ElfWonVikingLost(elfChoice, vikingChoice))
            {
                this._djViking.LostGame(vikingChoice);
                this._elf.WonGame(elfChoice);
            }
            if (this.VikingWonElfLost(elfChoice, vikingChoice))
            {
                this._djViking.WonGame(vikingChoice);
                this._elf.LostGame(elfChoice);
            }
        }
        public void PartOne()
        {
            for (int i = 0; i < this._lines.Length; i++)
            {
                char elfDraw = this._lines[i].Split(" ")[0].ToCharArray()[0];
                char vikingDraw = this._lines[i].Split(" ")[1].ToCharArray()[0];
                this.Step(elfDraw, vikingDraw);
            }
            Console.WriteLine("Part 1: {0}", this._djViking._score);
        }
        public void PartTwo()
        {
            Console.WriteLine("Part 2: ");
        }
    }
}

// |    elf        |      me       | score amount of outcome |      won 6     |     draw 3      |     lost 0   | 
// |  A - rock     |  X - rock     |           1             | 1 + won() => 7 | 1 + draw() => 4 | 1 + lost() 1 |  
// |  B - paper    |  Y - paper    |           2             | 2 + won() => 8 | 2 + draw() => 5 | 2 + lost() 2 |
// |  C - scissors |  Z - scissors |           3             | 3 + won() => 9 | 3 + draw() => 6 | 3 + lost() 3 |

// elf vs viking

// A rock     beats Z scissors
// B paper    beats X rock
// c scissors beats Y paper

// viking vs elf

// X rock     beats C scissors
// Y paper    beats A rock
// Z scissors beats B paper