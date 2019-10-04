//using System.Composition;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace CommandLineExtensibility
{
    //[Export(typeof(ICommand))]
    //[Export(typeof(SubCommand))]
    //[Export]
    public class BuiltinCommand : SubCommand//, ICommand<BuiltintCommandArguments>//, ICommand<ICommandArguments>
    {
        private readonly Configuration configuration;

        public override string Name => "builtin";

        public override string Description => "builtin command";

        //[ImportingConstructor]// FIXME shound't be mandatory
        public BuiltinCommand(/*Configuration configuration*/)
        {
            this.configuration = configuration;
        }

        public Task<int> Execute(BuiltintCommandArguments arguments)
        {
            //ICommand<ICommandArguments> toto = new BuiltinCommand(null);
            return Task.FromResult(configuration.GetHashCode());
        }

        public Task Execute()
        {
            throw new System.NotImplementedException();
        }

        public Task<int> Execute(ICommandArguments arguments)
        {
            throw new System.NotImplementedException();
        }
    }

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
