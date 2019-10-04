using System.Collections.Generic;
using System.CommandLine;

namespace CommandLineExtensibility
{
    public class BuiltintCommandArguments : ICommandArguments
    {
        public IEnumerable<Option> GetOptions()
        {
            return new[]
            {
                new Option("--int-option", "An option whose argument is parsed as an int")
                {
                    Argument = new Argument<int>(defaultValue: () => 42)
                }
            };
        }
    }
}
