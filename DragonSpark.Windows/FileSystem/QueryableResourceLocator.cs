using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Immutable;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace DragonSpark.Windows.FileSystem
{
	public class QueryableResourceLocator : ParameterizedSourceBase<string, ImmutableArray<string>>
	{
		readonly Func<FileSystemInfoBase, bool> specification;
		readonly DirectoryInfoBase directory;

		public QueryableResourceLocator( Func<FileSystemInfoBase, bool> specification ) : this( specification, new DirectoryInfo( "." ) ) {}

		public QueryableResourceLocator( Func<FileSystemInfoBase, bool> specification, DirectoryInfoBase directory )
		{
			this.specification = specification;
			this.directory = directory;
		}

		public override ImmutableArray<string> Get( string parameter ) => 
			directory.GetFileSystemInfos( parameter ).Where( specification ).Select( info => info.FullName ).ToImmutableArray();
	}
}