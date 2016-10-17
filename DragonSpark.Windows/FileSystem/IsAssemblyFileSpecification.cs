namespace DragonSpark.Windows.FileSystem
{
	public sealed class IsAssemblyFileSpecification : FileExtensionSpecificationBase
	{
		public static IsAssemblyFileSpecification Default { get; } = new IsAssemblyFileSpecification();
		IsAssemblyFileSpecification() : base( Defaults.AssemblyExtension ) {}
	}
}