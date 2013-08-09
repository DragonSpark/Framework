using DragonSpark.Io;
using DragonSpark.IoC;
using System.IO;

namespace DragonSpark.Application.Testing.Implementations
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
