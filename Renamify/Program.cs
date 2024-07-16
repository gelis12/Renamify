using Renamify;

const string HelpCommand = "-h";
const string RenameCommand = "-rename";

var renameCommand = new RenameCommand();

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
    switch (arg[0])
    {
        case RenameCommand:
            var commandArgs = GetCommandArguments();
            renameCommand.Apply(commandArgs);
            if (renameCommand.HasErrors)
            {
                DisplayRenameErrors();
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
}

void DisplayRenameErrors()
{
    foreach (var error in renameCommand.Errors)
    {
        Console.WriteLine(error);
    }
}

string[] GetCommandArguments()
{
    return arg.Skip(1).ToArray();
}

Console.ReadKey();