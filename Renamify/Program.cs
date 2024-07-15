using Renamify;

var renameCommand = new RenameCommand();
renameCommand.Apply(new string[] { "-p","C:\\Users\\eelis\\OneDrive\\Desktop\\testfile", "-f", "new", "-t", "old" });

Console.ReadKey();