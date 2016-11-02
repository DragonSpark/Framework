using DragonSpark.Sources.Parameterized;
using DragonSpark.Windows.FileSystem;
using System;
using FileInfo = System.IO.FileInfo;

namespace DragonSpark.Testing.Framework.FileSystem
{
	sealed class PathTranslator : ParameterizedSourceBase<FileInfo, string>
	{
		public static PathTranslator Default { get; } = new PathTranslator();
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