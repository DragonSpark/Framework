using DragonSpark.IoC;
using System.Web.Hosting;
using DragonSpark.Server.IO;

namespace DragonSpark.Server
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