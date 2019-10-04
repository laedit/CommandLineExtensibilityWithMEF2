using System;
using System.Collections.Generic;
using System.Composition.Hosting.Core;

namespace CommandLineExtensibility
{
    internal class InstanceExportDescriptorProvider : SinglePartExportDescriptorProvider
    {
        readonly object _exportedInstance;

        public InstanceExportDescriptorProvider(object exportedInstance, Type contractType, string contractName, IDictionary<string, object> metadata)
            : base(contractType, contractName, metadata)
        {
            _exportedInstance = exportedInstance ?? throw new ArgumentNullException("exportedInstance");
        }

        public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors(CompositionContract contract, DependencyAccessor descriptorAccessor)
        {
            if (IsSupportedContract(contract))
                yield return new ExportDescriptorPromise(contract, _exportedInstance.ToString(), true, NoDependencies, _ =>
                    ExportDescriptor.Create((c, o) => _exportedInstance, Metadata));
        }
    }
}
