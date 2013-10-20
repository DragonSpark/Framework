using System.IO;
using System.Reflection;
using System.Web.Hosting;

namespace DragonSpark.Server.ClientHosting
{
	public class EmbeddedResourceFile : VirtualFile
	{
		readonly Assembly assembly;
		readonly string name;

		public EmbeddedResourceFile( Assembly assembly, string name ) : base( Resolve( assembly, name ) )
		{
			this.assembly = assembly;
			this.name = name;
		}

		static string Resolve( Assembly assembly, string name )
		{
			var result = string.Concat( VirtualPathProvider.Directory, Path.AltDirectorySeparatorChar, assembly.GetName().Name, System.IO.Path.AltDirectorySeparatorChar, name );
			return result;
		}

		public override Stream Open()
		{
			var resource = string.Concat( assembly.GetName().Name, ".", name ).Replace( Path.AltDirectorySeparatorChar, '.' );
			var result = assembly.GetManifestResourceStream( resource );
			return result;
		}
	}
}