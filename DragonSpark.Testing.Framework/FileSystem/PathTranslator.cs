using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Windows.FileSystem;
using System;
using FileInfo = System.IO.FileInfo;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public sealed class CurrentDirectoryPath : Scope<string>
	{
		public static CurrentDirectoryPath Default { get; } = new CurrentDirectoryPath();
		CurrentDirectoryPath() : base( Windows.FileSystem.Defaults.CurrentPath.Wrap() ) {}
	}

	sealed class PathTranslator : ParameterizedSourceBase<FileInfo, string>
	{
		public static ISource<PathTranslator> Current { get; } = new Scope<PathTranslator>( Factory.GlobalCache( () => new PathTranslator() ) );
		PathTranslator() : this( Path.Default ) {}

		readonly IPath path;
		readonly Func<string> directoryName;

		public PathTranslator( IPath path ) : this( path, CurrentDirectoryPath.Default.Get ) {}

		public PathTranslator( IPath path, Func<string> directoryName )
		{
			this.path = path;
			this.directoryName = directoryName;
		}

		public override string Get( FileInfo parameter ) => path.Combine( directoryName(), parameter.Name );
	}
}