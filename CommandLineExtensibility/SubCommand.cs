namespace CommandLineExtensibility
{
    public abstract class SubCommand
    {
        public abstract string Name { get; }
     
        public abstract string Description { get; }
    }
}
