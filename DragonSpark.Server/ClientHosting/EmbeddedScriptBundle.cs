using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Optimization;
namespace DragonSpark.Server.ClientHosting
{
	[AttributeUsage( AttributeTargets.Assembly )]
	public class ClientApplicationAttribute : ClientResourcesAttribute
	{
		public ClientApplicationAttribute() : base( "application" )
		{}
	}

	[AttributeUsage( AttributeTargets.Assembly )]
	public class ClientResourcesAttribute : Attribute
	{
		readonly string name;

		public ClientResourcesAttribute( string name )
		{
			this.name = name;
		}

		public string Name
		{
			get { return name; }
		}
	}

	[AttributeUsage( AttributeTargets.Assembly, AllowMultiple = true )]
	public class ClientResourceAttribute : Attribute
	{
		public ClientResourceAttribute( string fileName )
		{
			FileName = fileName;
		}

		public string FileName { get; private set; }

		public string GetName( Assembly assembly )
		{
			var result = string.Concat( assembly.GetName().Name, '.', FileName.Replace( Path.AltDirectorySeparatorChar, '.' ) );
			return result;
		}
	}

	public class ClientResourcesBundle : ScriptBundle
	{
		public ClientResourcesBundle( string virtualPath = VirtualPathProvider.Path ) : base( virtualPath )
		{}

		public override IEnumerable<BundleFile> EnumerateFiles( BundleContext context )
		{
			var files = AppDomain.CurrentDomain.GetAssemblies().SelectMany( x => x.GetAttributes<ClientResourceAttribute>().Select( y => new { Assembly = x, Attribute = y } ) )
				.Select( x => new EmbeddedResourceFile( x.Assembly.GetManifestResourceStream( x.Attribute.GetName( x.Assembly ) ), string.Concat( Path, System.IO.Path.AltDirectorySeparatorChar, x.Assembly.GetName().Name, System.IO.Path.AltDirectorySeparatorChar, x.Attribute.FileName ) ) );
			var result = files.Select( x => new BundleFile( x.VirtualPath, x ) ).ToArray();
			return result;
		}
	}
}