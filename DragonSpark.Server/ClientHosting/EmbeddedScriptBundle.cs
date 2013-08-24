using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;
namespace DragonSpark.Server.ClientHosting
{
	[AttributeUsage( AttributeTargets.Assembly, AllowMultiple = true )]
	public class EmbeddedResourceAttribute : Attribute
	{
		public EmbeddedResourceAttribute( string filename )
		{
			Filename = filename;
		}

		public string Filename { get; set; }

		/*public string ResourceName { get; set; }*/

		/*public EmbeddedResourceAttribute(string filename, string resourceName)
		{
		  this.ResourceName = resourceName;
		}*/
	}

	public class EmbeddedScriptsBundle : ScriptBundle
	{
		public EmbeddedScriptsBundle( string virtualPath = VirtualPathProvider.Path ) : base( virtualPath )
		{}

		public override IEnumerable<BundleFile> EnumerateFiles( BundleContext context )
		{
			var files = AppDomain.CurrentDomain.GetAssemblies()
				.Where( x => x.IsDecoratedWith<EmbeddedResourceAttribute>() )
				.Select( x => x.FromMetadata<EmbeddedResourceAttribute, string>( y => y.Filename )
				.Transform( y => new EmbeddedResourceFile( x.GetManifestResourceStream( y ), string.Concat( Path, System.IO.Path.AltDirectorySeparatorChar, x.GetName().Name, System.IO.Path.AltDirectorySeparatorChar, y ) ) ) );
			var result = files.Select( x => new BundleFile( x.VirtualPath, x ) ).ToArray();
			return result;
		}
	}
}