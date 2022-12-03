namespace Day2
{
    class MainClass
    {
        public string input = "";
        public string[] _lines = new string[] { "" };
        public DJViking _djViking = new();
        public Dictionary<char, string> _charToName = new();
        public Dictionary<string, int> _nameToScore = new();
        public WinTable _winTable = new();
        public DrawTable _drawTable = new();
        public LoseTable _loseTable = new();
        public class WinTable : Dictionary<string, string>
        {
            public WinTable()
            {
                this["scissors"] = "rock";
                this["rock"] = "paper";
                this["paper"] = "scissors";
            }
        }
        public class DrawTable : Dictionary<string, string>
        {
            public DrawTable()
            {
                this["rock"] = "rock";
                this["paper"] = "paper";
                this["scissors"] = "scissors";
            }
        }
        public class LoseTable : Dictionary<string, string>
        {
            public LoseTable()
            {
                this["paper"] = "rock"; // rock loses to paper
                this["scissors"] = "paper";
                this["rock"] = "scissors";
            }
        }
        public class Player
        {
            public Dictionary<char, int> _choices = new();
            public double _score = 0;
            public void WonGame(char choice) { this._score += this._choices[choice] + 6; }
            public void DrawGame(char choice) { this._score += this._choices[choice] + 3; }
            public void LostGame(char choice) { this._score += this._choices[choice] + 0; }
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
            this._djViking = new DJViking();

            this._charToName['A'] = "rock";
            this._charToName['B'] = "paper";
            this._charToName['C'] = "scissors";

            this._charToName['X'] = "rock";
            this._charToName['Y'] = "paper";
            this._charToName['Z'] = "scissors";

            this._nameToScore["rock"] = 1;
            this._nameToScore["paper"] = 2;
            this._nameToScore["scissors"] = 3;


        }
        public void GetInput(string fileName)
        {
            this.input = File.ReadAllText(fileName);
            this._lines = input.Split(Environment.NewLine);
        }
        public bool DrawCase(char e, char v) { return (e == 'A' && v == 'X') || (e == 'B' && v == 'Y') || (e == 'C' && v == 'Z'); }
        public bool ElfWonVikingLost(char e, char v) { return (e == 'B' && v == 'X') || (e == 'A' && v == 'Z') || e == 'C' && v == 'Y'; }
        public bool VikingWonElfLost(char e, char v) { return (v == 'X' && e == 'C') || (v == 'Y' && e == 'A') || (v == 'Z' && e == 'B'); }
        public void StepPart1(char elfChoice, char vikingChoice)
        {
            if (this.DrawCase(elfChoice, vikingChoice))
            {
                this._djViking.DrawGame(vikingChoice);
            }
            if (this.ElfWonVikingLost(elfChoice, vikingChoice))
            {
                this._djViking.LostGame(vikingChoice);
            }
            if (this.VikingWonElfLost(elfChoice, vikingChoice))
            {
                this._djViking.WonGame(vikingChoice);
            }
        }
        public void StepPart2(char elfChoice, char vikingChoice)
        {
            if (this._charToName[elfChoice] == "rock")
            {
                switch (vikingChoice)
                {
                    case 'X': // lose
                        {
                            this._djViking._score += this._nameToScore[this._loseTable[this._charToName[elfChoice]]] + 0;
                        }
                        return;
                    case 'Y': // draw
                        {
                            this._djViking._score += this._nameToScore[this._drawTable[this._charToName[elfChoice]]] + 3;
                        }
                        return;
                    case 'Z': // win
                        {
                            this._djViking._score += this._nameToScore[this._winTable[this._charToName[elfChoice]]] + 6;
                        }
                        return;
                }
            }
            if (this._charToName[elfChoice] == "paper")
            {
                switch (vikingChoice)
                {
                    case 'X': // lose
                        {
                            this._djViking._score += this._nameToScore[this._loseTable[this._charToName[elfChoice]]] + 0;
                        }
                        return;
                    case 'Y': // draw
                        {
                            this._djViking._score += this._nameToScore[this._drawTable[this._charToName[elfChoice]]] + 3;
                        }
                        return;
                    case 'Z': // win
                        {
                            this._djViking._score += this._nameToScore[this._winTable[this._charToName[elfChoice]]] + 6;
                        }
                        return;
                }

            }
            if (this._charToName[elfChoice] == "scissors")
            {
                switch (vikingChoice)
                {
                    case 'X': // lose
                        {
                            this._djViking._score += this._nameToScore[this._loseTable[this._charToName[elfChoice]]] + 0;
                        }
                        return;
                    case 'Y': // draw
                        {
                            this._djViking._score += this._nameToScore[this._drawTable[this._charToName[elfChoice]]] + 3;
                        }
                        return;
                    case 'Z': // win
                        {
                            this._djViking._score += this._nameToScore[this._winTable[this._charToName[elfChoice]]] + 6;
                        }
                        return;
                }
            }
        }
        public void PartOne()
        {
            for (int i = 0; i < this._lines.Length; i++)
            {
                char elfChoice = this._lines[i].Split(" ")[0].ToCharArray()[0];
                char vikingChoice = this._lines[i].Split(" ")[1].ToCharArray()[0];
                this.StepPart1(elfChoice, vikingChoice);
            }
            Console.WriteLine("Part 1: {0}", this._djViking._score);
        }
        public void PartTwo()
        {
            for (int i = 0; i < this._lines.Length; i++)
            {
                char elfChoice = this._lines[i].Split(" ")[0].ToCharArray()[0];
                char vikingChoice = this._lines[i].Split(" ")[1].ToCharArray()[0];
                this.StepPart2(elfChoice, vikingChoice);
            }
            Console.WriteLine("Part 2: {0}", this._djViking._score);
        }
    }
}

// part 1
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


// part 2 

// |    elf        
// |  A - rock     1
// |  B - paper    2
// |  C - scissors 3

// second column decides how I should proceed
// X means I should lose
// Y means I should draw
// Z means I should win 