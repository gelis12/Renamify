namespace Renamify
{
    public class TrimCommand
    {
        private string _path;
        private int _trimStart;
        private int _trimEnd;

        private const string PathArg = "-p";
        private const string TrimStartArg = "-trim--start";
        private const string TrimEndArg = "-trim--end";
        private const int InvalidTrimValue = -1;

        private Dictionary<string, string> _allowedArguments = new Dictionary<string, string>()
        {
            { PathArg, "Full path of the folder to be used"},
            { TrimStartArg, "Specify a number of chars to be trimmed from the start of the file name"},
            { TrimEndArg, "Specify a number of chars to be trimmed from the end of the file name" }
        };

        public List<string> Errors { get; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;

        public void Apply(string[] arg)
        {
            Build(arg);

            if (!ArgumentsAreValid())
            {
                Console.WriteLine("No trimming done! ");
                return;
            }

            var files = Directory.GetFiles(_path);

            foreach (string file in files)
            {
                if (!File.Exists(file))
                {
                    continue;
                }

                var currentFile = file;
                var trimmedFile = GetTrimmedFileName(currentFile);
                try
                {
                    File.Move(currentFile, trimmedFile);
                }
                catch (Exception ex)
                {
                    Errors.Add($"Error trimming [{currentFile}]: {ex.Message}");
                    continue;
                }
                Console.WriteLine($"File was trimmed {_trimStart} chars at the start amd {_trimEnd} chars at the end");
            }
        }

        public void Describe()
        {
            Console.WriteLine("Trim: use -trim to trim a specified number of chars at the start and/or end of the file name");
            Console.WriteLine("Allowed arguments");
            foreach (var arg in _allowedArguments)
            {
                Console.WriteLine($"{arg.Key} : {arg.Value}");
            }
        }

        private void Build(string[] arg)
        {
            var argumentsPairs = PairArgumentsWithValues(arg);

            foreach (var argument in argumentsPairs)
            {
                switch (argument.Key)
                {
                    case PathArg:
                        _path = argument.Value;
                        break;
                    case TrimStartArg:
                        _trimStart = ParseIntArgValue(argument.Key, argument.Value);
                        break;
                    case TrimEndArg:
                        _trimEnd = ParseIntArgValue(argument.Key, argument.Value);
                        break;
                    default:
                        Errors.Add($"Invalid argument: {argument.Key}");
                        break;
                }
            }

        }

        private Dictionary<string, string> PairArgumentsWithValues(string[] arg)
        {
            var argumentsPairs = new Dictionary<string, string>();

            if (IsOddNumber(arg.Length))
            {
                Errors.Add("Invalid list of arguments");
                return argumentsPairs;
            }

            for (int i = 0; i < arg.Length; i++)
            {
                var isLastArgument = i == arg.Length - 1;
                if (!isLastArgument)
                {
                    argumentsPairs.Add(arg[i], arg[i + 1]);
                }
                i++;
            }
            return argumentsPairs;
        }

        private static bool IsOddNumber(int number)
        {
            return number % 2 != 0;
        }

        private bool ArgumentsAreValid()
        {
            if (HasErrors)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(_path))
            {
                Errors.Add($"Path is needed, use {PathArg} argument to add it");
                return false;
            }
            if (_trimStart == 0 && _trimEnd == 0)
            {
                return false;
            }
            
            return true;

        }

        private int ParseIntArgValue(string arg, string argValue)
        {
            if (!int.TryParse(argValue, out int intValue))
            {
                Errors.Add($"Value {argValue} is invalid for {arg}");
                return InvalidTrimValue;
            }
            return intValue;
        }

        private string GetTrimmedFileName(string filePath)
        {
            string trimmedFileName;
            try
            {
                var initialFileName = filePath.Split('\\').LastOrDefault().Split(".").FirstOrDefault();
                trimmedFileName = initialFileName.Substring(_trimStart, initialFileName.Length - _trimStart);
                trimmedFileName = trimmedFileName.Substring(0, trimmedFileName.Length - _trimEnd);
                trimmedFileName = filePath.Replace(initialFileName, trimmedFileName);
            }
            catch (Exception ex)
            {
                Errors.Add($"Error trimming file {filePath}: {ex.Message}");
                return filePath;
            }
            return trimmedFileName;
        }
    }
}
