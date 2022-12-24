namespace Day7
{
    class MainClass
    {
        public string input = "";
        public Dir _rootDir = new Dir("/", null);
        public Dir bestDir = new Dir("/", null);
        public double _part1MaxSize = 100000;
        public double _totalDiskSize = 70000000;
        public double _requiredDiskSpace = 30000000;
        public double usedSize = 0;
        public double missingSize = 0;
        public double unusedSize = 0;
        public Dir? _currentDir = null;
        public List<Dir> _smallDirs = new();
        public string[] _lines = new string[] { "" };
        public class Dir
        {
            public Dir(string dirName, Dir? parent)
            {
                this._dirName = dirName;
                this._parent = parent;
            }
            public string _dirName { get; set; } = "";
            public List<Dir> _dirs { get; } = new();
            public Dir? _parent { get; set; }

            public List<MyFile> _files = new();
            public bool HasDir(string dirName) => this._dirs.Any(d => d._dirName == dirName);
            public bool HasFile(string fileName) => this._files.Any(f => f._name == fileName);
            // cached value instead of computing everytime
            private double? _totalSize = null;
            public double totalDirSize
            {
                get
                {
                    double? size = 0;
                    if (this._totalSize is not null)
                    {
                        return this._totalSize.Value;
                    }
                    foreach (MyFile file in _files)
                    {
                        size += file._size;
                    }
                    foreach (Dir dir in _dirs)
                    {
                        size += dir.totalDirSize;
                    }
                    this._totalSize = size;
                    return this._totalSize.Value;
                }
            }
        }
        public record MyFile(string _name, double _size);
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
            this._smallDirs = new List<Dir>();
            this._rootDir = new Dir("/", null);
            this.bestDir = this._rootDir;
            this._currentDir = null;
        }
        public void GetInput(string fileName)
        {
            this.input = File.ReadAllText(fileName);
            this._lines = input.Split(Environment.NewLine);
        }
        public void ParseCli()
        {
            foreach (string line in this._lines)
            {
                var lineSplit = line.Split(" ");
                if (line.StartsWith("$"))
                {
                    // command
                    if (lineSplit[1] == "cd")
                    {
                        if (lineSplit[2] == "/")
                        {
                            this._currentDir = this._rootDir;
                        }
                        else if (lineSplit[2] == "..")
                        {
                            this._currentDir = this._currentDir!._parent;
                        }
                        else
                        {
                            string targetName = lineSplit[2];
                            // if the current directory doesn't have any directories with the current name of the output dir name
                            // create it and then 
                            if (this._currentDir!._dirs.FirstOrDefault(x => x._dirName == targetName) is not { } targetDirectory)
                            {
                                Dir newDir = new Dir(targetName, this._currentDir);
                                this._currentDir._dirs.Add(newDir);
                                targetDirectory = newDir;
                            }
                            // go into that directory in this cd command;
                            this._currentDir = targetDirectory;
                        }
                    }
                }
                else
                {
                    //sub dir output
                    if (lineSplit[0] == "dir")
                    {
                        if (!this._currentDir!.HasDir(lineSplit[1]))
                        {
                            Dir newDir = new Dir(lineSplit[1], this._currentDir);
                            this._currentDir._dirs.Add(newDir);
                        }
                    }
                    else
                    {
                        if (!this._currentDir!.HasFile(lineSplit[1]))
                        {
                            MyFile newFile = new MyFile(lineSplit[1], double.Parse(lineSplit[0]));
                            this._currentDir._files.Add(newFile);
                        }
                    }
                }
            }
        }
        public void TraverseDirs1(Dir source)
        {
            if (source.totalDirSize <= this._part1MaxSize)
            {
                this._smallDirs.Add(source);
            }
            foreach (Dir dir in source._dirs)
            {
                this.TraverseDirs1(dir);
            }
        }
        public void TraverseDirs2(Dir source)
        {
            if (this.missingSize <= source.totalDirSize &&
                this.bestDir.totalDirSize > source.totalDirSize)
            {
                this.bestDir = source;
            }

            foreach (Dir dir in source._dirs)
            {
                this.TraverseDirs2(dir);
            }
        }
        public void PartOne()
        {
            this.ParseCli();
            this.TraverseDirs1(this._rootDir);
            Console.WriteLine("Part 1: {0}", this._smallDirs.Sum(d => d.totalDirSize));
        }
        public void PartTwo()
        {
            this.ParseCli();

            this.usedSize = this._rootDir.totalDirSize;
            this.unusedSize = this._totalDiskSize - this.usedSize;
            this.missingSize = this._requiredDiskSpace - this.unusedSize;

            this.TraverseDirs2(this._rootDir);

            Console.WriteLine("Part 2: {0}", this.bestDir.totalDirSize);
        }
    }
}
