using System.IO;
using System.Web.Hosting;
using DragonSpark.Extensions;

namespace DragonSpark.Io
{
	partial class PathHelper
	{
		static string ResolvePathInternal( string filePath )
		{
			var result = filePath.Transform( x => HostingEnvironment.MapPath( x ) ?? Path.GetFullPath( x ) );
			return result;
		}
	}
}
