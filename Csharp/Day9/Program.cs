namespace Day9
{
    class MainClass
    {
        public record struct Point(int X, int Y)
        {
            public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);

            public static Point operator -(Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);

            public Point Normalize() => new Point(X != 0 ? X / Math.Abs(X) : 0, Y != 0 ? Y / Math.Abs(Y) : 0);

            public static implicit operator Point((int X, int Y) tuple) => new Point(tuple.X, tuple.Y);

            public int ManhattanDistance(Point b) => Math.Abs(X - b.X) + Math.Abs(Y - b.Y);
        }
        public Point[] rope = new Point[10];
        public Point tailPosition = new();
        public Point headPosition = new();
        public HashSet<Point> visited1 = new();
        public HashSet<Point> visited2 = new();
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
            this.visited1 = new HashSet<Point>();
            this.visited2 = new HashSet<Point>();
            this.visited1.Add(tailPosition);
            this.visited2.Add(rope[9]);
        }
        public void GetInput(string fileName)
        {
            this.input = File.ReadAllText(fileName);
            this._lines = input.Split(Environment.NewLine);
        }
        public void ParseMovements1()
        {
            foreach (string line in this._lines)
            {
                string[] movementParts = line.Split(" ");
                Point direction = movementParts[0] switch
                {
                    "U" => new Point(0, 1),
                    "D" => new Point(0, -1),
                    "R" => new Point(1, 0),
                    "L" => new Point(-1, 0),
                    _ => throw new Exception("Invalid Direction"),
                };

                int distance = int.Parse(movementParts[1]);

                for (int i = 0; i < distance; i++)
                {
                    this.headPosition += direction;
                    if (Math.Abs(this.headPosition.X - this.tailPosition.X) > 1
                        || Math.Abs(this.headPosition.Y - this.tailPosition.Y) > 1)
                    {
                        if (this.headPosition.X != this.tailPosition.X)
                        {
                            int xDiff = (this.headPosition.X - this.tailPosition.X) / Math.Abs(this.headPosition.X - this.tailPosition.X);
                            this.tailPosition.X += xDiff;
                        }

                        if (this.headPosition.Y != this.tailPosition.Y)
                        {
                            int yDiff = (this.headPosition.Y - this.tailPosition.Y) / Math.Abs(this.headPosition.Y - this.tailPosition.Y);
                            this.tailPosition.Y += yDiff;
                        }

                        this.visited1.Add(this.tailPosition);
                    }
                }
            }
        }
        public void ParseMovements2()
        {
            foreach (string movement in this._lines)
            {
                string[] movementParts = movement.Split(" ");
                Point direction = movementParts[0] switch
                {
                    "U" => new Point(0, 1),
                    "D" => new Point(0, -1),
                    "R" => new Point(1, 0),
                    "L" => new Point(-1, 0),
                    _ => throw new Exception("Invalid direction"),
                };

                int distance = int.Parse(movementParts[1]);

                for (int i = 0; i < distance; i++)
                {
                    this.rope[0] += direction;

                    for (int knot = 1; knot < 10; knot++)
                    {
                        Point headPosition = this.rope[knot - 1];
                        Point currentPosition = this.rope[knot];

                        if (Math.Abs(headPosition.X - currentPosition.X) > 1
                            || Math.Abs(headPosition.Y - currentPosition.Y) > 1)
                        {
                            if (headPosition.X != currentPosition.X)
                            {
                                int xDiff = (headPosition.X - currentPosition.X) / Math.Abs(headPosition.X - currentPosition.X);
                                currentPosition.X += xDiff;
                            }

                            if (headPosition.Y != currentPosition.Y)
                            {
                                int yDiff = (headPosition.Y - currentPosition.Y) / Math.Abs(headPosition.Y - currentPosition.Y);
                                currentPosition.Y += yDiff;
                            }

                            this.rope[knot] = currentPosition;

                            if (knot == 9)
                            {
                                this.visited2.Add(currentPosition);
                            }
                        }
                    }
                }
            }
        }
        public void PartOne()
        {
            this.ParseMovements1();
            Console.WriteLine("Part 1: {0}", this.visited1.Count());
        }
        public void PartTwo()
        {
            this.ParseMovements2();
            Console.WriteLine("Part 2: {0}", this.visited2.Count());
        }
    }
}
