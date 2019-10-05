using System;
using System.CommandLine.Invocation;
using System.CommandLine;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Composition;
using System.Linq;

namespace CommandLineExtensibility
{
    internal sealed class Starter
    {
        private readonly RootCommand rootCommand;

        public Starter()
        {
            // Create a root command with some options
            rootCommand = new RootCommand
            {
                new Option(
                    new[] {"-s", "--source" },
                    "site source folder")
                {
                    Argument = new Argument<string>(defaultValue: () => Environment.CurrentDirectory)
                },
                new Option(
                    new[] { "-d", "--debug" },
                    "debug")
                {
                    Argument = new Argument<bool>()
                }
            };

            rootCommand.Description = "Dotnet Static site generator";

            rootCommand.TreatUnmatchedTokensAsErrors = false;

            rootCommand.Handler = CommandHandler.Create<string, bool>((source, debug) =>
            {
                Console.WriteLine($"The value for --source is: {source}");
                Console.WriteLine($"The value for --debug is: {debug}");
            });
        }

        //[ImportMany("Command")]
        [ImportMany]
        public IEnumerable<ICommand> Commands { get; set; }

        //[ImportMany("Command")]
        [ImportMany]
        public IEnumerable<ICommand<ICommandArguments>> CommandsWithArgs { get; set; }

        [ImportMany]
        public IEnumerable<ICommand<BuiltintCommandArguments>> CommandsWithClosedArgs { get; set; }

        [ImportMany]
        public IEnumerable<SubCommand> SubCommands { get; set; }

        [ImportMany]
        public IEnumerable<Command> RealCommands { get; set; }

        [ImportMany]
        public IEnumerable<ICommandArgumentsExtension> CommandArgExtensions { get; set; }

        public Task<int> Start(string[] args)
        {
            // Parse the incoming args and invoke the handler
            return rootCommand.InvokeAsync(args);
        }

        [OnImportsSatisfied]
        internal void ImportSatisfied()
        {
            foreach (var command in RealCommands)
            {
                rootCommand.AddCommand(command);
            }
        }
    }
}
