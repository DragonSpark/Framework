namespace DragonSpark.Windows.FileSystem
{
	public sealed class AssemblyFilePathSelector : QueryableResourceLocator
	{
		public AssemblyFilePathSelector() : base( IsAssemblyFileSpecification.Default.IsSatisfiedBy ) {}
	}
}
