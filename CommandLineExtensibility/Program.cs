using System;
using System.CommandLine;
using System.Composition;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace CommandLineExtensibility
{
    internal static class Program
    {
        static Task<int> Main(string[] args)
        {
            Console.WriteLine(IsDescendentOf(typeof(BuiltinCommand), typeof(ICommand<>)));
            //return Task.FromResult(1);

            var safeArg = new Option("--safe") { Argument = new Argument<bool>() };
            var isSafe = safeArg.Parse(string.Join(" ", args)).ValueForOption<bool>("safe");

            // MEF initialization
            var container = new ContainerConfiguration()
                                            //.WithMetadata<BuiltinCommand>("QueueName", "StockMovements") // example of metadata handling
                                            .WithAssembly(Assembly.GetExecutingAssembly(), GetInheritedCommandsConvention())//GetInheritedConvention(typeof(ICommand<>)))//SubCommand
                                            .WithPlugins(isSafe)
                                            .WithExport(Configuration.ReadFrom(Environment.CurrentDirectory))
                                            //.WithInheritedPart<ICommand<>>()
                                            //.WithInheritedPart<SubCommand>()
                                            .WithParts(new[] { typeof(ICommand), typeof(ICommand<>), typeof(BuiltinCommand) })
                                            .CreateContainer();

            var starter = new Starter();

            container.SatisfyImports(starter);

            return starter.Start(args);
        }
        private static bool IsDescendentOf(Type type, Type baseType)
        {
            if (type == baseType || type == typeof(object) || type == null)
            {
                return false;
            }
            TypeInfo typeInfo = IntrospectionExtensions.GetTypeInfo(type);
            TypeInfo typeInfo2 = IntrospectionExtensions.GetTypeInfo(baseType);
            if (((Type)(object)typeInfo).IsGenericTypeDefinition || ((Type)(object)typeInfo2).IsGenericTypeDefinition)
            {
                return IsGenericDescendentOf(typeInfo, typeInfo2);
            }
            return typeInfo2.IsAssignableFrom(typeInfo);
        }
        private static bool IsGenericDescendentOf(TypeInfo derivedType, TypeInfo baseType)
        {
            if (((Type)(object)derivedType).BaseType == null)
            {
                return false;
            }
            if (((Type)(object)derivedType).BaseType == baseType.AsType())
            {
                return true;
            }
            foreach (Type implementedInterface in derivedType.ImplementedInterfaces)
            {
                if (implementedInterface.IsConstructedGenericType && implementedInterface.GetGenericTypeDefinition() == baseType.AsType())
                {
                    return true;
                }
            }
            return IsGenericDescendentOf(IntrospectionExtensions.GetTypeInfo(((Type)(object)derivedType).BaseType), baseType);
        }

        // 1. Add new Command without args
        // 2. Add new Command with args
        // 3. Add existing Command arg
        // 4. Add other plugins (transform, engine, etc.)
        // 5. Add loading from 'plugins' folder
        
        public static ContainerConfiguration WithPlugins(this ContainerConfiguration config, bool isSafe)
        {
            return isSafe || !Directory.Exists("_plugins")
                ? config
                : config.WithAssemblies(Directory
                .GetFiles("_plugins", "*.dll", SearchOption.AllDirectories)
                .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath));
        }

        public static AttributedModelProvider GetInheritedCommandsConvention()
        {
            var conventions = new ConventionBuilder();
            conventions.ForTypesDerivedFrom(typeof(Command))
                .Export(ecb => ecb.AsContractType(typeof(Command)));

            return conventions;


            //var conventions = new ConventionBuilder();
            //conventions.ForTypesDerivedFrom(typeof(ICommand<>))
            ////.ExportInterfaces(i =>
            ////{
            ////    return i.GetGenericTypeDefinition() == typeof(ICommand<>);
            ////}, (type, builder) => builder.AsContractType(typeof(ICommand<>)))
            //.Export(ecb => ecb.AsContractType(typeof(ICommand<>)))
            //.SelectConstructor(ctors => ctors.First());

            ////.SelectConstructor(c => c.Single())
            ////.Export();

            ////.Export(ecb => ecb.AsContractType(typeof(ICommand<ICommandArguments>))
            ///*.AsContractName("Command")*/ //);

            //return conventions;
        }

        public static AttributedModelProvider GetInheritedConvention<T>()
        {
            return GetInheritedConvention(typeof(T));
            //var conventions = new ConventionBuilder();
            //conventions.ForTypesDerivedFrom<T>().Export(ecb => ecb.AsContractType<T>());

            //return conventions;
        }

        public static AttributedModelProvider GetInheritedConvention(Type contractType)
        {
            var conventions = new ConventionBuilder();
            conventions.ForTypesDerivedFrom(contractType).Export(ecb => ecb.AsContractType(contractType));

            return conventions;
        }

        public static ContainerConfiguration WithInheritedPart(this ContainerConfiguration config, Type contractType)
        {
            var conventions = GetInheritedConvention(contractType);

            return config.WithPart(contractType, conventions);
        }

        public static ContainerConfiguration WithInheritedPart<T>(this ContainerConfiguration config)
        {
            //return config.WithInheritedPart(typeof(T));

            var conventions = GetInheritedConvention<T>();

            return config.WithPart<T>(conventions);
        }

        public static ContainerConfiguration WithMetadata<TContract, TMetadata>(this ContainerConfiguration config, string metadataName, TMetadata metadataValue)
        {
            var conventions = new ConventionBuilder();

            conventions
                .ForType<TContract>()
                .Export<TContract>(x => x.AddMetadata(metadataName, metadataValue));

            return config.WithPart<TContract>(conventions);
        }

    }
}
