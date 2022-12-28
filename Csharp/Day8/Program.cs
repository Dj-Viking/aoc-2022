namespace Day8
{
    class MainClass
    {
        public string input = "";
        public string[] _lines = new string[] { "" };
        public double VisibleTrees = 0;
        public class EdgeType
        {
            public static readonly string corner = "corner";
            public static readonly string side = "side";
            public static readonly string top_bottom = "top_bottom";
        }
        public List<List<int>> InputGrid = new();
        public List<Tree> TreeList = new();
        public static bool IsNotEdge(Tree tree)
        {
            return tree.Edge[tree.Signature] is null;
        }
        public static bool IsInBounds(int index, dynamic list)
        {
            if (list is List<List<int>>)
            {
                return (index >= 0) && (index < ((List<List<int>>)list).Count());
            }
            if (list is List<int>)
            {
                return (index >= 0) && (index < ((List<int>)list).Count());
            }
            return false;
        }
        public class Tree
        {
            public Dictionary<string, string?> Edge { get; set; } = new();
            public Dictionary<string, Dictionary<string, List<int>?>> AdjacencyMapHeightList { get; set; } = new();
            public int[] Coordinates { get; set; } = new int[] { 0, 0 }; // [x, y] or [r, c]
            public string Signature { get; set; } = "";
            public int Height = 0;
            public Tree(int height, int row, int col, List<List<int>> inputGrid)
            {
                this.Signature = $"{height}-[{row}, {col}]";
                this.Height = height;

                this.Coordinates[0] = row;
                this.Coordinates[1] = col;

                this.InitEdge(this.Signature, row, col, inputGrid);
                this.InitAdjacencyMapHeightList(this.Signature, row, col, inputGrid);
            }
            public void InitAdjacencyMapHeightList(string sig, int row, int col, List<List<int>> graph)
            {

                this.AdjacencyMapHeightList.Add(sig, new Dictionary<string, List<int>?>());

                this.AdjacencyMapHeightList[sig].Add("up", new List<int>());
                this.AdjacencyMapHeightList[sig].Add("down", new List<int>());
                this.AdjacencyMapHeightList[sig].Add("left", new List<int>());
                this.AdjacencyMapHeightList[sig].Add("right", new List<int>());

                int? up = IsInBounds(row - 1, graph[row]) ? graph[row - 1][col] : null;
                int? down = IsInBounds(row + 1, graph[row]) ? graph[row + 1][col] : null;
                int? left = IsInBounds(col - 1, graph) ? graph[row][col - 1] : null;
                int? right = IsInBounds(col + 1, graph) ? graph[row][col + 1] : null;

                if (up is not null)
                {
                    for (int i = row - 1; i >= 0; i--)
                    {
                        if (IsNotEdge(this))
                        {
                            this.AdjacencyMapHeightList[sig]["up"]!.Add(graph[i][col]);
                        }
                    }
                }
                else this.AdjacencyMapHeightList[sig]["up"] = null;

                if (down is not null)
                {
                    for (int i = row + 1; i < graph.Count(); i++)
                    {
                        if (IsNotEdge(this))
                        {
                            this.AdjacencyMapHeightList[sig]["down"]!.Add(graph[i][col]);
                        }
                    }
                }
                else this.AdjacencyMapHeightList[sig]["down"] = null;

                if (left is not null)
                {
                    for (int i = col - 1; i >= 0; i--)
                    {
                        if (IsNotEdge(this))
                        {
                            this.AdjacencyMapHeightList[sig]["left"]!.Add(graph[row][i]);
                        }
                    }
                }
                else this.AdjacencyMapHeightList[sig]["left"] = null;

                if (right is not null)
                {
                    for (int i = col + 1; i < graph[0].Count(); i++)
                    {
                        if (IsNotEdge(this))
                        {
                            this.AdjacencyMapHeightList[sig]["right"]!.Add(graph[row][i]);
                        }
                    }
                }
                else this.AdjacencyMapHeightList[sig]["right"] = null;

            }
            public void InitEdge(string sig, int row, int col, List<List<int>> graph)
            {
                if ((col == 0 || col == graph[row].Count() - 1)
                    && (row == 0 || row == graph.Count() - 1))
                {
                    this.Edge[sig] = EdgeType.corner;
                }
                else if ((col == 0 || col == graph[row].Count() - 1)
                    && (row > 0 || row == graph.Count() - 2))
                {
                    this.Edge[sig] = EdgeType.side;
                }
                else if ((row == 0 || row == graph.Count() - 1)
                    && (col > 0 || col < graph[row].Count() - 1))
                {
                    this.Edge[sig] = EdgeType.top_bottom;
                }
                else this.Edge[sig] = null;
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
        public void GetInput(string fileName)
        {
            this.input = File.ReadAllText(fileName);
            this._lines = input.Split(Environment.NewLine);
        }
        public void Init()
        {
            this.input = "";
            this._lines = new string[] { "" };
            this.InputGrid = new List<List<int>>();
            this.TreeList = new List<Tree>();
        }
        public void InitInputGrid()
        {
            foreach (string line in this._lines)
            {
                List<char> lineChars = new();
                foreach (char chr in line)
                {
                    lineChars.Add(chr);
                }
                this.InputGrid.Add(lineChars.Select(x => int.Parse(x.ToString())).ToList());
            }
        }
        public void DebugInputGrid()
        {
            foreach (List<int> row in this.InputGrid)
            {
                Console.WriteLine("row [{0}]", string.Join(", ", row));
            }
        }
        public void CreateTreesFromGrid()
        {
            for (int r = 0; r < this.InputGrid.Count(); r++)
            {
                for (int c = 0; c < this.InputGrid[r].Count(); c++)
                {
                    int height = this.InputGrid[r][c];
                    Tree tree = new Tree(height, r, c, this.InputGrid);
                    this.TreeList.Add(tree);
                }
            }
        }
        public void CheckIfTreesAreVisible()
        {
            List<Tree> notEdges = new();
            foreach (Tree tree in this.TreeList)
            {
                if (!IsNotEdge(tree)) this.VisibleTrees++;

                if (IsNotEdge(tree))
                {
                    notEdges.Add(tree);
                }
            }

            foreach (Tree tree in notEdges)
            {
                bool upVisible = false;
                bool downVisible = false;
                bool leftVisible = false;
                bool rightVisible = false;

                List<int> upList = tree.AdjacencyMapHeightList[tree.Signature]["up"]!;
                List<int> downList = tree.AdjacencyMapHeightList[tree.Signature]["down"]!;
                List<int> leftList = tree.AdjacencyMapHeightList[tree.Signature]["left"]!;
                List<int> rightList = tree.AdjacencyMapHeightList[tree.Signature]["right"]!;

                // if tree is visible from top and left || top and right || bottom and left || bottom and right
                // this.visible++

                foreach (int height in upList)
                {
                    if (height < tree.Height) upVisible = true;
                    else
                    {
                        upVisible = false;
                        break;
                    }
                }
                foreach (int height in downList)
                {
                    if (height < tree.Height) downVisible = true;
                    else
                    {
                        downVisible = false;
                        break;
                    }
                }
                foreach (int height in leftList)
                {
                    if (height < tree.Height) leftVisible = true;
                    else
                    {
                        leftVisible = false;
                        break;
                    }
                }
                foreach (int height in rightList)
                {
                    if (height < tree.Height) rightVisible = true;
                    else
                    {
                        rightVisible = false;
                        break;
                    }
                }

                if (upVisible)
                {
                    this.VisibleTrees++;
                    continue;
                }
                if (rightVisible)
                {
                    this.VisibleTrees++;
                    continue;
                }
                if (downVisible)
                {
                    this.VisibleTrees++;
                    continue;
                }
                if (leftVisible)
                {
                    this.VisibleTrees++;
                    continue;
                }
            }
        }
        public double GetHighestScenicTreeScore()
        {
            List<Tree> notEdges = new();
            List<double> scoreList = new();
            foreach (Tree tree in this.TreeList)
            {
                if (!IsNotEdge(tree)) this.VisibleTrees++;

                if (IsNotEdge(tree))
                {
                    notEdges.Add(tree);
                }
            }

            foreach (Tree tree in notEdges)
            {

                long upAmountVisible = 0;
                long downAmountVisible = 0;
                long leftAmountVisible = 0;
                long rightAmountVisible = 0;

                List<int> upList = tree.AdjacencyMapHeightList[tree.Signature]["up"]!;
                List<int> downList = tree.AdjacencyMapHeightList[tree.Signature]["down"]!;
                List<int> leftList = tree.AdjacencyMapHeightList[tree.Signature]["left"]!;
                List<int> rightList = tree.AdjacencyMapHeightList[tree.Signature]["right"]!;

                Dictionary<string, List<long>> treeScenicMap = new();

                treeScenicMap.Add("up", new List<long>());
                treeScenicMap.Add("down", new List<long>());
                treeScenicMap.Add("left", new List<long>());
                treeScenicMap.Add("right", new List<long>());

                // if tree is visible from top and left || top and right || bottom and left || bottom and right
                // this.visible++

                foreach (int height in upList)
                {
                    if (height < tree.Height)
                    {
                        upAmountVisible++;
                    }
                    else if (height == tree.Height)
                    {
                        upAmountVisible++;
                        break;
                    }
                    else if (height > tree.Height)
                    {
                        upAmountVisible++;
                        break;
                    }
                }

                treeScenicMap["up"].Add(upAmountVisible);

                foreach (int height in downList)
                {
                    if (height < tree.Height)
                    {
                        downAmountVisible++;
                    }
                    else if (height == tree.Height)
                    {
                        downAmountVisible++;
                        break;
                    }
                    else if (height > tree.Height)
                    {
                        downAmountVisible++;
                        break;
                    }
                }

                treeScenicMap["down"].Add(downAmountVisible);

                foreach (int height in leftList)
                {
                    if (height < tree.Height)
                    {
                        leftAmountVisible++;
                    }
                    else if (height == tree.Height)
                    {
                        leftAmountVisible++;
                        break;
                    }
                    else if (height > tree.Height)
                    {
                        leftAmountVisible++;
                        break;
                    }
                }

                treeScenicMap["left"].Add(leftAmountVisible);

                foreach (int height in rightList)
                {
                    if (height < tree.Height)
                    {
                        rightAmountVisible++;
                    }
                    else if (height == tree.Height)
                    {
                        rightAmountVisible++;
                        break;
                    }
                    else if (height > tree.Height)
                    {
                        rightAmountVisible++;
                        break;
                    }
                }

                treeScenicMap["right"].Add(rightAmountVisible);

                double scenicScore = 1;

                foreach (List<long> numList in treeScenicMap.Values)
                {
                    foreach (long num in numList)
                    {
                        scenicScore *= num;
                    }
                    scoreList.Add(scenicScore);
                }

            }
            return scoreList.Max();
        }
        public void PartOne()
        {
            this.InitInputGrid();
            this.CreateTreesFromGrid();
            this.CheckIfTreesAreVisible();
            Console.WriteLine("Part 1: {0}", this.VisibleTrees);
        }
        public void PartTwo()
        {
            this.InitInputGrid();
            this.CreateTreesFromGrid();
            Console.WriteLine("Part 2: {0}", this.GetHighestScenicTreeScore());
        }
    }
}

/* 
    A tree is visible if all of the other trees between it and an edge of the grid are shorter than it.

    Only consider trees in the same row or column; that is, only look up, down, left, or right from any given tree.
*/
