//using System.Composition;
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
}
