// using System.Text.RegularExpressions;
namespace Day7
{
    class MainClass
    {
        public string input = "";
        public string[] _lines = new string[] { "" };
        public LinkedList<string> _linkedList = new();
        public string _currentPath = "";
        public class MyFile
        {
            public double _size = 0;
            public string _name = "";
            public MyFile(string name, double size)
            {
                this._name = name;
                this._size = size;
            }
        }
        public class Output
        {
            public List<string> _children = new();
            public string _dirPath = "";
            public List<MyFile> _files = new();

            public Output(string dirPath)
            {
                this._dirPath = dirPath;
            }
        }
        public Dictionary<string, Output> _terminalTree = new();
        public record DIR(string _dirPath, double _size);
        public class Dir
        {
            public string _dirPath = "";
            public double _size = 0;
            public Dir(string dirPath, double size)
            {
                this._dirPath = dirPath;
                this._size = size;
            }
        }
        public List<Dir> _dirSizes = new();
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
            this._currentPath = "";
            this._lines = new string[] { "" };
            this._terminalTree = new Dictionary<string, Output>();
            this._dirSizes = new List<Dir>();
        }
        public void AllocateTree()
        {
            this._linkedList = new(this._lines);
        }
        public void ParseTree()
        {

            string currentDir = "";
            string rootDir = this._linkedList.ElementAt(0).Split(" ")[2];
            // start off the current path with root dir /
            this._currentPath = rootDir;
            this._terminalTree[rootDir] = new Output(rootDir);
            Console.WriteLine("***** start reading file system");
            for (int i = 0; i < this._linkedList.Count(); i++)
            {
                Console.WriteLine("---- line item in linked list {0}", this._linkedList.ElementAt(i));

                if (this._linkedList.ElementAt(i).Contains("$ cd .."))
                {
                    // un append the current dir from the current path string to simulate changing directories one level up the tree
                    string tempPath = "";
                    string[] splitCurrentPath = this._currentPath.Split("/", StringSplitOptions.RemoveEmptyEntries);

                    if (splitCurrentPath.Length > 1)
                    {
                        for (int k = 0; k < splitCurrentPath.Length - 1; k++)
                        {
                            tempPath += $"/{splitCurrentPath[k]}";
                        }
                        this._currentPath = tempPath;
                    }
                    else if (splitCurrentPath.Length == 1)
                    {
                        this._currentPath = "/";
                    }


                    continue;
                }

                if (this._linkedList.ElementAt(i).Contains("$ cd"))
                {
                    // multiple dirs can have the same names so we need to include them in the entire terminal tree
                    // append the dir we changed into on the current path string

                    currentDir = this._linkedList.ElementAt(i).Split(" ")[2];
                    if (currentDir != rootDir)
                    {
                        if (this._currentPath.Split("/", StringSplitOptions.RemoveEmptyEntries).Length == 0)
                        {
                            this._currentPath += $"{currentDir}";
                        }
                        else
                        {
                            this._currentPath += $"/{currentDir}";
                        }
                    }

                    continue;
                }

                if (this._linkedList.ElementAt(i).Contains("$ ls"))
                {
                    Console.WriteLine("------ user typed ls!!!!");
                    for (int j = (i + 1); j < this._linkedList.Count(); j++)
                    {
                        string tempDirname2 = "";
                        string tempDirPath = this._currentPath;

                        if (this._linkedList.ElementAt(j).Contains("$")) break;

                        if (this._linkedList.ElementAt(j).Contains("dir"))
                        {
                            tempDirname2 = this._linkedList.ElementAt(j).Split(" ")[1];
                            if (this._currentPath != rootDir)
                            {
                                tempDirPath += $"/{tempDirname2}";
                            }
                            else
                            {
                                tempDirPath += $"{tempDirname2}";
                            }

                            if (!this._terminalTree.ContainsKey(tempDirPath))
                            {
                            }

                            this._terminalTree.TryAdd(tempDirPath, new Output(tempDirPath));

                            if (!this._terminalTree[this._currentPath]._children.Any(x => x == tempDirPath))
                            {
                                if (this._currentPath != rootDir)
                                {
                                    this._terminalTree[this._currentPath]._children.Add(tempDirPath);
                                }
                                else
                                {
                                    this._terminalTree[rootDir]._children.Add(tempDirPath);
                                }
                            }
                        }

                        double fileSize = 0;
                        if (double.TryParse(this._linkedList.ElementAt(j).Split(" ")[0], out fileSize))
                        {
                            string fileName = this._linkedList.ElementAt(j).Split(" ")[1];

                            this._terminalTree[this._currentPath]._files.Add(new MyFile(fileName, fileSize));
                            Console.WriteLine("YYY reading terminal tree");
                            this.DebugTerminalTree();
                            Console.WriteLine("YYY end reading terminal tree");
                        }

                    }
                }
            }
            Console.WriteLine("***** end reading file system");
        }
        public void DebugTerminalTree()
        {
            foreach (Output output in this._terminalTree.Values)
            {
                Console.WriteLine(" --- current dir path {0} ", output._dirPath);
                foreach (MyFile file in this._terminalTree[output._dirPath]!._files)
                {
                    Console.WriteLine("-- has this file --\n filename: {0} - filesize: {1}\nCCCC dir {3} has children [{2}]", file._name, file._size, string.Join(", ", this._terminalTree[output._dirPath]._children), output._dirPath);
                }
            }
        }
        public void CalculateDirSizes()
        {
            foreach (Output output in this._terminalTree.Values)
            {
                double sum = 0;

                foreach (MyFile file in this._terminalTree[output._dirPath]._files)
                {
                    sum += file._size;
                }

                if (this._terminalTree[output._dirPath]._children.Count() > 0)
                {
                    foreach (string childDirPath in this._terminalTree[output._dirPath]._children)
                    {
                        foreach (MyFile file2 in this._terminalTree[childDirPath]._files)
                        {
                            sum += file2._size;
                        }
                    }
                }
                this._dirSizes.Add(new Dir(output._dirPath, sum));
            }
            // this.DebugDirSizes();
        }
        public void DebugDirSizes()
        {
            foreach (Dir dir in this._dirSizes)
            {
                Console.WriteLine(" in dir {0} there is size {1}", dir._dirPath, dir._size);
            }
        }
        public double GetSumOfDirsLessThan100k()
        {
            List<double> dirs = new();

            foreach (Dir dir in this._dirSizes)
            {
                if (dir._size <= 100000)
                {
                    dirs.Add(dir._size);
                }
            }
            Console.WriteLine("------ showing dirs less than 100000 [{0}]", string.Join(", ", dirs.Distinct()));

            return dirs.Distinct().Sum();
        }
        public void PartOne()
        {
            this.AllocateTree();
            this.ParseTree();
            this.CalculateDirSizes();
            Console.WriteLine("Part 1: {0}", this.GetSumOfDirsLessThan100k());
        }
        public void PartTwo()
        {
            Console.WriteLine("Part 2: {0}", "answer goes here");
        }
    }
}
