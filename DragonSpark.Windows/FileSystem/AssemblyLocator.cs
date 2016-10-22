namespace DragonSpark.Windows.FileSystem
{
	public sealed class AssemblyLocator : QueryableResourceLocator
	{
		// public static IParameterizedSource<string, ImmutableArray<string>> Default { get; } = new AssemblyLocator().ToEqualityCache();
		public AssemblyLocator() : base( IsAssemblyFileSpecification.Default.IsSatisfiedBy ) {}
	}
}
