using Renamify;

const string HelpCommand = "-h";
const string RenameCommand = "-rename";
const string TrimCommand = "-trim";

var renameCommand = new RenameCommand();
var trimCommand  = new TrimCommand();

var arg = args;

if (arg.Length == 0)
{
    return;
}

if (arg.Contains(HelpCommand))
{
    DisiplayAvailableCommands();
}
else
{
    var commandArgs = GetCommandArguments();
    switch (arg[0])
    {
        case RenameCommand:
            renameCommand.Apply(commandArgs);
            if (renameCommand.HasErrors)
            {
                DisplayErrors();
            }
            break;
        case TrimCommand:
            trimCommand.Apply(commandArgs);
            if (trimCommand.HasErrors)
            {
                DisplayErrors();
            }
            break;
        default:
            Console.WriteLine("Invalid command");
            break;
    }
}

void DisiplayAvailableCommands()
{
    Console.WriteLine("Renamify - command line interface for handling file groups");
    Console.WriteLine("Renamify supports following commands:");
    Console.WriteLine("--------------------------------------");
    renameCommand.Describe();
    trimCommand.Describe();
}

void DisplayErrors()
{
    foreach (var error in renameCommand.Errors)
    {
        Console.WriteLine(error);
    }
    foreach (var error in trimCommand.Errors)
    {
        Console.WriteLine(error);
    }
}

string[] GetCommandArguments()
{
    return arg.Skip(1).ToArray();
}

Console.ReadKey();