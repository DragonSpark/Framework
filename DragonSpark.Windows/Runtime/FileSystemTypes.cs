namespace DragonSpark.Windows.Runtime
{
	public sealed class FileSystemTypes : ApplicationTypesBase
	{
		public static FileSystemTypes Default { get; } = new FileSystemTypes();
		FileSystemTypes() : base( FileSystemAssemblySource.Default.Get ) {}
	}
}