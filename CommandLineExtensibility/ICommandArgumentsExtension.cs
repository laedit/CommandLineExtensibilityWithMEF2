using System;
using System.Collections.Generic;
using System.CommandLine;

namespace CommandLineExtensibility
{
    interface ICommandArgumentsExtension
    {
        Type For();

        IEnumerable<Option> GetArguments();
    }
}
