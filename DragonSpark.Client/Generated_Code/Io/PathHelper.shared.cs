using System.IO;

namespace DragonSpark.Io
{
	public static partial class PathHelper
	{
        public static string ResolvePath( string filePath )
		{
			// First check with HostingEnvironment:
			var result = Path.IsPathRooted( filePath ) ? filePath : ResolvePathInternal( filePath );
			return result;
		}
	}
}