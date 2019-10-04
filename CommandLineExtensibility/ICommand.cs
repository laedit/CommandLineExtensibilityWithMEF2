using System.Threading.Tasks;

namespace CommandLineExtensibility
{
    public interface ICommand
    {
        string Name { get; }

        string Description { get; }

        Task Execute();
    }

    public interface ICommand<T> : ICommand where T : ICommandArguments
    {
        Task<int> Execute(T arguments);
    }
}
