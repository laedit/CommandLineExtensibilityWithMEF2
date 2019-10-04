using System;
using System.Collections.Generic;
using System.Composition.Hosting;

namespace CommandLineExtensibility
{
    // From https://github.com/microsoftarchive/mef/tree/master/oob/demo/Microsoft.Composition.Demos.ExtendedPartTypes
    // Discovered through https://github.com/dotnet/corefx/issues/37249#issuecomment-489885424
    internal static class ContainerConfigurationExtensions
    {
        public static ContainerConfiguration WithExport<T>(this ContainerConfiguration configuration, T exportedInstance, string contractName = null, IDictionary<string, object> metadata = null)
        {
            return WithExport(configuration, exportedInstance, typeof(T), contractName, metadata);
        }

        public static ContainerConfiguration WithExport(this ContainerConfiguration configuration, object exportedInstance, Type contractType, string contractName = null, IDictionary<string, object> metadata = null)
        {
            return configuration.WithProvider(new InstanceExportDescriptorProvider(
                exportedInstance, contractType, contractName, metadata));
        }
    }
}
