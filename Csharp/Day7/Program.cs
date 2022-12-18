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
        public class Output
        {
            public List<string> _children = new();
            public string _dirname = "";
            public List<MyFile> _files = new();

            public Output(string dirname) {
                this._dirname = dirname;
            }
        }
        public List<Output> _terminalTree = new();
        public class Dir {
            public string _dirname = "";
            public double _size = 0;
            public Dir(string dirname, double size) {
                this._dirname = dirname;
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
            this._lines = new string[] { "" };
            this._terminalTree = new List<Output>();
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
            Console.WriteLine("***** start reading file system");
            for (int i = 0; i < this._linkedList.Count(); i++)
            {
                Console.WriteLine("---- line item in linked list {0}", this._linkedList.ElementAt(i));

                if (this._linkedList.ElementAt(i).Contains("$ cd ..")) continue;

                if (this._linkedList.ElementAt(i).Contains("$ cd"))
                {
                    // multiple dirs can have the same names so we need to include them in the entire terminal tree
                    currentDir = this._linkedList.ElementAt(i).Split(" ")[2];
                    this._terminalTree.Add(new Output(currentDir));
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
                            this._terminalTree.Add(new Output(tempDirname2));
                            this._terminalTree.Find(x => x._dirname == currentDir)!._children.Add(tempDirname2);
                            this._terminalTree.Find(x => x._dirname == rootDir)!._children.Add(tempDirname2);
                        }

                        double fileSize = 0;
                        if (double.TryParse(this._linkedList.ElementAt(j).Split(" ")[0], out fileSize))
                        {
                            string fileName = this._linkedList.ElementAt(j).Split(" ")[1];
                            MyFile file = new MyFile(fileName, fileSize);

                            this._terminalTree.Find(x => x._dirname == currentDir)!._files.Add(file);
                            Console.WriteLine("YYY reading terminal tree");
                            // this.DebugTerminalTree();
                            Console.WriteLine("YYY end reading terminal tree");
                        }

                    }
                }
            }
            Console.WriteLine("***** end reading file system");
        }
        public void DebugTerminalTree()
        {
            foreach (Output output in this._terminalTree)
            {
                Console.WriteLine(" --- current dir {0} ", output._dirname);
                foreach (MyFile file in this._terminalTree.Find(x => x._dirname == output._dirname)!._files)
                {
                    Console.WriteLine("-- has this file --\n filename: {0} - filesize: {1}\nCCCC dir {3} has children [{2}]", file._name, file._size, string.Join(", ", this._terminalTree.Find(x => x._dirname == output._dirname)!._children), output._dirname);
                }
            }
        }
        public void CalculateDirSizes()
        {
            foreach (Output output in this._terminalTree)
            {
                double sum = 0;

                foreach (MyFile file in this._terminalTree.Find(x => x._dirname == output._dirname)!._files)
                {
                    sum += file._size;
                }

                if (this._terminalTree.Find(x => x._dirname == output._dirname)!._children.Count() > 0)
                {
                    foreach (string childDir in this._terminalTree.Find(x => x._dirname == output._dirname)!._children)
                    {
                        foreach (MyFile file2 in this._terminalTree.Find(x => x._dirname == childDir)!._files)
                        {
                            sum += file2._size;
                        }
                    }
                }
                this._dirSizes.Add(new Dir(output._dirname, sum));
            }
            // this.DebugDirSizes();
        }
        public void DebugDirSizes()
        {
            foreach (Dir dir in this._dirSizes)
            {
                Console.WriteLine(" in dir {0} there is size {1}", dir._dirname, dir._size);
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
