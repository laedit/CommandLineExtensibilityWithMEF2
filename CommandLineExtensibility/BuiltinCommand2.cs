//using System.Composition;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

namespace CommandLineExtensibility
{
    public class BuiltinCommand2 : Command
    {
        public BuiltinCommand2()
            : base("builtin2", "builtin2 command")
        {
            this.AddOption(new Option("--output") { Argument = new Argument<FileInfo>() });
            this.AddOption(new Option(
                    new[] { "-d", "--debug" },
                    "debug")
            {
                Argument = new Argument<bool>()
            });

            Handler = CommandHandler.Create((FileInfo output, bool debug) => 
            { 
                System.Console.WriteLine($"Output path: {output}");
                System.Console.WriteLine($"The value for --debug is: {debug}");
            });
        }
    }
}
