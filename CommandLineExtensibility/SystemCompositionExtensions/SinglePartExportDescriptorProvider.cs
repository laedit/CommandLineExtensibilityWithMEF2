using System;
using System.Collections.Generic;
using System.Composition.Hosting.Core;
using System.Linq;

namespace CommandLineExtensibility
{
    internal abstract class SinglePartExportDescriptorProvider : ExportDescriptorProvider
    {
        readonly Type _contractType;
        readonly string _contractName;

        protected SinglePartExportDescriptorProvider(Type contractType, string contractName, IDictionary<string, object> metadata)
        {
            _contractType = contractType ?? throw new ArgumentNullException("contractType");
            _contractName = contractName;
            Metadata = metadata ?? new Dictionary<string, object>();
        }

        protected bool IsSupportedContract(CompositionContract contract)
        {
            if (contract.ContractType != _contractType ||
                contract.ContractName != _contractName)
                return false;

            if (contract.MetadataConstraints != null)
            {
                var subsetOfConstraints = contract.MetadataConstraints.Where(c => Metadata.ContainsKey(c.Key)).ToDictionary(c => c.Key, c => Metadata[c.Key]);
                var constrainedSubset = new CompositionContract(contract.ContractType, contract.ContractName,
                    subsetOfConstraints.Count == 0 ? null : subsetOfConstraints);

                if (!contract.Equals(constrainedSubset))
                    return false;
            }

            return true;
        }

        protected IDictionary<string, object> Metadata { get; }
    }
}
