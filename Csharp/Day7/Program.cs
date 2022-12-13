// using System.Text.RegularExpressions;
namespace Day7
{
    class MainClass
    {
        public string input = "";
        public string[] _lines = new string[] { "" };
        public LinkedList<string> _linkedList = new();
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
        public class Output : List<MyFile>
        {
            public HashSet<string> _children = new();
        }
        public Dictionary<string, Output> _terminalTree = new();
        public Dictionary<string, double> _dirSizes = new();
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
            this._terminalTree = new Dictionary<string, Output>();
            this._dirSizes = new Dictionary<string, double>();
        }
        public void AllocateTree()
        {
            this._linkedList = new(this._lines);
        }
        public void ParseTree()
        {

            string currentDir = "";
            string rootDir = this._linkedList.ElementAt(0).Split(" ")[2];
            Console.WriteLine("***** start reading file system");
            for (int i = 0; i < this._linkedList.Count(); i++)
            {
                Console.WriteLine("---- line item in linked list {0}", this._linkedList.ElementAt(i));

                if (this._linkedList.ElementAt(i).Contains("$ cd ..")) continue;

                if (this._linkedList.ElementAt(i).Contains("$ cd"))
                {
                    // if dir already exists then don't add it to the terminal tree
                    currentDir = this._linkedList.ElementAt(i).Split(" ")[2];
                    this._terminalTree.TryAdd(currentDir, new Output());
                    continue;
                }

                if (this._linkedList.ElementAt(i).Contains("$ ls"))
                {
                    Console.WriteLine("------ user typed ls!!!!");
                    for (int j = (i + 1); j < this._linkedList.Count(); j++)
                    {
                        string tempDirname2 = "";
                        if (this._linkedList.ElementAt(j).Contains("$")) break;

                        if (this._linkedList.ElementAt(j).Contains("dir"))
                        {
                            tempDirname2 = this._linkedList.ElementAt(j).Split(" ")[1];
                            this._terminalTree.TryAdd(tempDirname2, new Output());
                            this._terminalTree[currentDir]._children.Add(tempDirname2);
                            this._terminalTree[rootDir]._children.Add(tempDirname2);
                        }

                        double fileSize = 0;
                        if (double.TryParse(this._linkedList.ElementAt(j).Split(" ")[0], out fileSize))
                        {
                            string fileName = this._linkedList.ElementAt(j).Split(" ")[1];
                            MyFile file = new MyFile(fileName, fileSize);

                            this._terminalTree[currentDir].Add(file);
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
            foreach (string dir in this._terminalTree.Keys)
            {
                Console.WriteLine(" --- current dir {0} ", dir);
                foreach (MyFile file in this._terminalTree[dir])
                {
                    Console.WriteLine("-- has this file --\n filename: {0} - filesize: {1}\nCCCC dir {3} has children [{2}]", file._name, file._size, string.Join(", ", this._terminalTree[dir]._children), dir);
                }
            }
        }
        public void CalculateDirSizes()
        {
            foreach (string dir in this._terminalTree.Keys)
            {
                double sum = 0;

                foreach (MyFile file in this._terminalTree[dir])
                {
                    sum += file._size;
                }

                if (this._terminalTree[dir]._children.Count() > 0)
                {
                    foreach (string childDir in this._terminalTree[dir]._children)
                    {
                        foreach (MyFile file2 in this._terminalTree[childDir])
                        {
                            sum += file2._size;
                        }
                    }
                }
                this._dirSizes.TryAdd(dir, sum);
            }
            this.DebugDirSizes();
        }
        public void DebugDirSizes()
        {
            foreach (string dir in this._dirSizes.Keys)
            {
                Console.WriteLine(" in dir {0} there is size {1}", dir, this._dirSizes[dir]);
            }
        }
        public void PartOne()
        {
            this.AllocateTree();
            this.ParseTree();
            this.CalculateDirSizes();
            Console.WriteLine("Part 1: {0}", "answer goes here");
        }
        public void PartTwo()
        {
            Console.WriteLine("Part 2: {0}", "answer goes here");
        }
    }
}
