using System.IO;
using System.Reflection;
using System.Web.Hosting;
using DragonSpark.Extensions;
using DragonSpark.Logging;

namespace DragonSpark.Server.ClientHosting
{
	public class ClientResourceFile : VirtualFile
	{
		readonly IClientResourceStreamProvider provider;
		readonly string path;

		public ClientResourceFile( IClientResourceStreamProvider provider, string path ) : base( path )
		{
			this.provider = provider;
			this.path = path;
		}

		/*static string Resolve( Assembly assembly, string name )
		{
			var result = string.Concat( VirtualPathProvider.Directory, Path.AltDirectorySeparatorChar, assembly.GetName().Name, Path.AltDirectorySeparatorChar, name );
			return result;
		}

		public override Stream Open()
		{
			var resource = string.Concat( assembly.GetName().Name, ".", name ).Replace( Path.AltDirectorySeparatorChar, '.' );
			var result = assembly.GetManifestResourceStream( resource );
			result.Null( () => Log.Warning( string.Format( "Stream not found for {0}, in assembly {1}.", name, assembly.GetName() ) ) );
			return result;
		}*/
		public override Stream Open()
		{
			var result = provider.Retrieve( path );
			return result;
		}
	}
}