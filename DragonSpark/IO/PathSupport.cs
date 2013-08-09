using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System.IO;

namespace DragonSpark.Io
{
	public static class PathSupport
	{
		public static string ResolvePath( string filePath )
		{
			var result = Path.IsPathRooted( filePath ) ? filePath : filePath.Transform( x => ServiceLocation.With<IPathResolver, string>( y => y.Resolve( x ) ) ?? Path.GetFullPath( x ) );
			return result;
		}
	}
}
