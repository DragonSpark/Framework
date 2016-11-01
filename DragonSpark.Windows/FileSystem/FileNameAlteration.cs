using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using JetBrains.Annotations;

namespace DragonSpark.Windows.FileSystem
{
	public sealed class FileNameAlteration : DelegatedAlteration<string>
	{
		public static ISource<FileNameAlteration> Current { get; } = new Scope<FileNameAlteration>( Sources.Factory.GlobalCache( () => new FileNameAlteration() ) );
		FileNameAlteration() : this( Path.Default ) {}

		[UsedImplicitly]
		public FileNameAlteration( IPath path ) : base( path.GetFileName ) {}
	}
}
