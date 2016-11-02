using JetBrains.Annotations;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public sealed class InitializeFileSystemAttribute : RegisterDirectoriesAttribute
	{
		public InitializeFileSystemAttribute() : this( DirectorySource.Default.Get() ) {}

		[UsedImplicitly]
		public InitializeFileSystemAttribute( string rootDirectory ) : base( rootDirectory ) {}
	}
}