//-p "path" -f "new" -t "old"
using System.ComponentModel;

namespace Renamify
{
    public class RenameCommand
    {
        private string _path;
        private string _renameFrom;
        private string _renameTo;

        private const string PathArg = "-p";
        private const string FromArg = "-f";
        private const string ToArg = "-t";

        public List<string> Errors { get; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;

        public void Apply(string[] arg) 
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
                if(!isLastArgument)
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
