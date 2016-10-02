using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace DragonSpark.Windows
{
	public class QueryableResourceLocator : ParameterizedSourceBase<string, ImmutableArray<string>>
	{
		readonly Func<FileSystemInfo, bool> specification;
		public QueryableResourceLocator( Func<FileSystemInfo, bool> specification )
		{
			this.specification = specification;
		}

		public override ImmutableArray<string> Get( string parameter ) => 
			new DirectoryInfo( "." ).GetFileSystemInfos( parameter ).Where( specification ).Select( info => info.FullName ).ToImmutableArray();
	}
}