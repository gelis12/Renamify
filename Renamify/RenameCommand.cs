//-p "path" -f "new" -t "old"
using System.ComponentModel;

namespace Renamify
{
    public class RenameCommand : BaseCommand
    {
        private string _path;
        private string _renameFrom;
        private string _renameTo;

        private const string PathArg = "-p";
        private const string FromArg = "-f";
        private const string ToArg = "-t";

        public override Dictionary<string, string> AllowedArguments { get; } = new Dictionary<string, string>() 
        {
            { PathArg, "Full path of the folder to be used"},
            { FromArg, "Text to rename from"},
            { ToArg, "Text to rename to" }
        };

        public override void Apply(string[] arg) 
        {
            Build(arg);

            if (!ArgumentsAreValid())
            {
                return;
            }

            var files = Directory.GetFiles(_path);

            foreach (string file in files)
            {
                if (!File.Exists(file)) 
                { 
                    continue;
                }
                var currentFileName = file;
                var newFileName = currentFileName.Replace(_renameFrom, _renameTo, StringComparison.OrdinalIgnoreCase);

                if (string.Equals(currentFileName, newFileName,StringComparison.Ordinal))
                {
                    continue;
                }

                try
                {
                    File.Move(currentFileName, newFileName);
                }
                catch (Exception ex)
                {
                    Errors.Add($"Error renaming [{currentFileName}] to [{newFileName}]: {ex.Message}");
                    continue;
                }
                Console.WriteLine($"Renamed file [{currentFileName}] to [{newFileName}]");
            }
        }
        public override void Describe() 
        {
            Console.WriteLine("Rename: use -rename to rename all files inside a specified folder");
            DescribeArguments();
        }

        private void Build(string[] arg)
        {
            var argumentsPairs = PairArgumentsWithValues(arg);

            foreach (var item in argumentsPairs)
            {
                switch (item.Key)
                {
                    case PathArg:
                        _path = item.Value;
                        break;
                    case FromArg:
                        _renameFrom = item.Value;
                        break;
                    case ToArg:
                        _renameTo = item.Value;
                        break;
                    default:
                        Errors.Add($"Invalid argument: {item.Key}");
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
            if (string.IsNullOrWhiteSpace(_renameFrom))
            {
                Errors.Add($"Text to rename is needed, use {FromArg} argument to add it");
                return false;
            }
            if (string.IsNullOrWhiteSpace(_renameTo))
            {
                Errors.Add($"Text to rename to is needed, use {ToArg} argument to add it");
                return false;
            }
            return true;
            
        }
    }
}
