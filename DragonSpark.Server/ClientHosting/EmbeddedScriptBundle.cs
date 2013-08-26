using DragonSpark.Client;
using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace DragonSpark.Server.ClientHosting
{
	public class ClientResourcesBundle : ScriptBundle
	{
		public ClientResourcesBundle( string virtualPath = VirtualPathProvider.Path ) : base( virtualPath )
		{}

		public override IEnumerable<BundleFile> EnumerateFiles( BundleContext context )
		{
			var files = AppDomain.CurrentDomain.GetAssemblies().Where( x => x.IsDecoratedWith<ClientResourcesAttribute>() ).SelectMany( x => x.GetAttributes<ClientResourceAttribute>().OrderByDescending( y => y.Priority )
					.Select( y => new { Assembly = x, Attribute = y } ) )
					.Select( x => new EmbeddedResourceFile( x.Assembly.GetManifestResourceStream( x.Attribute.GetName( x.Assembly ) ), string.Concat( Path, System.IO.Path.AltDirectorySeparatorChar, x.Assembly.GetName().Name, System.IO.Path.AltDirectorySeparatorChar, x.Attribute.FileName ) ) );
			var result = files.Select( x => new BundleFile( x.VirtualPath, x ) ).ToArray();
			return result;
		}
	}
}