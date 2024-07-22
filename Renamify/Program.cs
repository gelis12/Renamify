using Renamify;

const string HelpCommand = "-h";
const string RenameCommand = "-rename";
const string TrimCommand = "-trim";

var arg = args;

var renameCommand = new RenameCommand();
var trimCommand  = new TrimCommand();

var commands = new List<BaseCommand>() 
{
    renameCommand,
    trimCommand
};



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
    switch (arg[0])
    {
        case RenameCommand:
            Execute(renameCommand);
            break;
        case TrimCommand:
            Execute(trimCommand);
            break;
        default:
            Console.WriteLine("Invalid command");
            break;
    }
}

void Execute(BaseCommand command)
{
    var commandArgs = GetCommandArguments();
    command.Apply(commandArgs);
    if (renameCommand.HasErrors)
    {
        DisplayErrors(command);
    }
}

void DisiplayAvailableCommands()
{
    Console.WriteLine("Renamify - command line interface for handling file groups");
    Console.WriteLine("Renamify supports following commands:");
    Console.WriteLine("--------------------------------------");
    foreach (var command in commands)
    {
        command.Describe();
    }
}

void DisplayErrors(BaseCommand command)
{
    foreach (var error in command.Errors)
    {
        Console.WriteLine(error);
    }
    
}

string[] GetCommandArguments()
{
    return arg.Skip(1).ToArray();
}

Console.ReadKey();
