namespace Renamify
{
    public class TrimCommand : BaseCommand
    {
        private string _path;
        private int _trimStart;
        private int _trimEnd;

        private const string PathArg = "-p";
        private const string TrimStartArg = "--trim-start";
        private const string TrimEndArg = "--trim-end";
        private const int InvalidTrimValue = -1;

        public override Dictionary<string, string> AllowedArguments { get; } = new Dictionary<string, string>()
        {
            { PathArg, "Full path of the folder to be used"},
            { TrimStartArg, "Specify a number of chars to be trimmed from the start of the file name"},
            { TrimEndArg, "Specify a number of chars to be trimmed from the end of the file name" }
        };

        public override void Apply(string[] arg)
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

        public override void Describe()
        {
            Console.WriteLine("Trim: use -trim to trim a specified number of chars at the start and/or end of the file name");
            DescribeArguments();
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
