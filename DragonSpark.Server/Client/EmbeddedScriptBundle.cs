using DragonSpark.Extensions;
using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace DragonSpark.Server.Client
{
	public class EmbeddedScriptsBundle : ScriptBundle
	{
		public EmbeddedScriptsBundle( string virtualPath = VirtualPathProvider.Path ) : base( virtualPath )
		{}

		public override IEnumerable<BundleFile> EnumerateFiles( BundleContext context )
		{
			var files = AppDomain.CurrentDomain.GetAssemblies()
				.Where( x => x.IsDecoratedWith<JsEmbeddedResourceAttribute>() )
				.Select( x => x.FromMetadata<JsEmbeddedResourceAttribute, string>( y => y.Filename )
				.Transform( y => new EmbeddedResourceFile( x.GetManifestResourceStream( y ), string.Concat( Path, System.IO.Path.AltDirectorySeparatorChar, x.GetName().Name, System.IO.Path.AltDirectorySeparatorChar, y ) ) ) );
			var result = files.Select( x => new BundleFile( x.VirtualPath, x ) ).ToArray();
			return result;
		}
	}
}