using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Windows.FileSystem
{
	public class QueryableResourceLocator : ParameterizedSourceBase<string, ImmutableArray<string>>
	{
		readonly Func<IFileSystemInfo, bool> specification;
		readonly IDirectoryInfo directory;

		public QueryableResourceLocator( Func<IFileSystemInfo, bool> specification ) : this( specification, DirectoryInfoFactory.Default.Get( Defaults.CurrentPath ) ) {}

		public QueryableResourceLocator( Func<IFileSystemInfo, bool> specification, IDirectoryInfo directory )
		{
			this.specification = specification;
			this.directory = directory;
		}

		public override ImmutableArray<string> Get( string parameter )
		{
			var fileSystemInfos = directory.GetFileSystemInfos( parameter );
			return fileSystemInfos.Where( specification ).Select( info => info.FullName ).ToImmutableArray();
		}
	}
}