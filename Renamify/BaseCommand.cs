namespace Renamify
{
    public abstract class BaseCommand
    {
        public List<string> Errors { get; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;

        public abstract Dictionary<string, string> AllowedArguments { get; }

        public abstract void Describe();
        public abstract void Apply(string[] arg);

        protected Dictionary<string, string> PairArgumentsWithValues(string[] arg)
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

        protected static bool IsOddNumber(int number)
        {
            return number % 2 != 0;
        }

        protected void DescribeArguments()
        {
            Console.WriteLine("Allowed arguments");
            foreach (var arg in AllowedArguments)
            {
                Console.WriteLine($"{arg.Key} : {arg.Value}");
            }
        }



        
    }
}
