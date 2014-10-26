using System.IO;
using DragonSpark.Io;
using DragonSpark.IoC;

namespace DragonSpark.Application.Server.Testing.Implementations
{
	[Singleton(typeof(IPathResolver))]
	public class PathResolver : IPathResolver
	{
		public string Resolve( string path )
		{
			var result = Path.GetFullPath( path.Replace( "~", System.Environment.CurrentDirectory ) );
			return result;
		}
	}
}
