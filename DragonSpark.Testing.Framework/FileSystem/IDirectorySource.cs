using DragonSpark.Sources;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public interface IDirectorySource : IAssignableSource<string> {}
	sealed class DirectorySource : SuppliedSource<string>, IDirectorySource
	{
		public static IScope<IDirectorySource> Current { get; } = new Scope<IDirectorySource>( Factory.GlobalCache( () => new DirectorySource() ) );
		DirectorySource() : base( Defaults.DirectoryName ) {}
	}
}