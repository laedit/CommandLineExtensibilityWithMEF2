using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Composition;

namespace CommandLineExtensibility
{
    [Export(typeof(ICommandArgumentsExtension))]
    class Builtin2ArgsExt : ICommandArgumentsExtension
    {
        public Type For() => typeof(BuiltinCommand2);

        public IEnumerable<Option> GetArguments()
        {
            return new[]
            {
                new Option("--do")
                {
                    Argument = new Argument<bool>()
                }
            };
        }
    }
}
