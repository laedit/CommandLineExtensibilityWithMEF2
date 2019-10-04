using System.Collections.Generic;
using System.CommandLine;

namespace CommandLineExtensibility
{
    public interface ICommandArguments
    {
        IEnumerable<Option> GetOptions();
    }
}
