namespace DragonSpark.Windows
{
	public sealed class IsAssemblyFileSpecification : FileExtensionSpecificationBase
	{
		public static IsAssemblyFileSpecification Default { get; } = new IsAssemblyFileSpecification();
		IsAssemblyFileSpecification() : base( FileSystem.AssemblyExtension ) {}
	}
}