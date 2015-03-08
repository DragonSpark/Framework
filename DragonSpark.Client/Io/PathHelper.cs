using System.IO;

namespace DragonSpark.Io
{
	partial class PathHelper
	{
		static string ResolvePathInternal( string filePath )
		{
			var result = Path.GetFullPath( filePath );
			return result;
		}
	}
}
