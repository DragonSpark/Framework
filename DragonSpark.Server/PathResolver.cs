using System.Web.Hosting;
using DragonSpark.Server.Legacy.Io;

namespace DragonSpark.Server.Legacy
{
	[Singleton(typeof(IPathResolver))]
	public class PathResolver : IPathResolver
	{
		public string Resolve( string path )
		{
			var result = HostingEnvironment.MapPath( path );
			return result;
		}
	}
}