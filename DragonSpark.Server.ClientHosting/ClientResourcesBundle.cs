using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Services;
using System.IO;
using System.Linq;
using System.Web.Optimization;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.IoC.Configuration;
using DragonSpark.Runtime;
using Activator = DragonSpark.Runtime.Activator;

namespace DragonSpark.Server.ClientHosting
{
	public interface IClientResourceLocator
	{
		IEnumerable<Uri> LocateAll();
	}

	[Singleton( typeof(IClientResourceLocator) )]
	class ClientResourceLocator : IClientResourceLocator
	{
		readonly IEnumerable<ClientResourceManifest> manifests;

		public ClientResourceLocator( IEnumerable<ClientResourceManifest> manifests )
		{
			this.manifests = manifests ?? Enumerable.Empty<ClientResourceManifest>();
		}

		public IEnumerable<Uri> LocateAll()
		{
			var result = manifests.OrderBy( x => x.GetType().Assembly.FromMetadata<RegistrationAttribute, Priority>( y => y.Priority, () => Priority.Normal ) ).SelectMany( x => x.Preferred.Union( x ) ).Select( x => x.Location ).ToArray();
			return result;
		}
	}

	public class ClientResource
	{
		public Uri Location { get; set; } 
	}

	public class ClientResourceManifest : List<ClientResource>
	{
		public ICollection<ClientResource> Preferred
		{
			get { return preferred; }
		}	readonly ICollection<ClientResource> preferred = new List<ClientResource>();
	}


	public class AssemblyClientResourceManifest : ClientResourceManifest
	{
		public AssemblyClientResourceManifest()
		{
			var assembly = GetType().Assembly;
			var name = assembly.GetName().Name;
			assembly.GetResourceNames().Where( x => Path.GetExtension( x ) == ".js" ).Select( x => new Uri( string.Format( "pack://application:,,,/{0};component/{1}", name, x ) ) ).Select( x => new ClientResource { Location = x } ).Apply( Add );
		}
	}


	/*public class ClientResourcesBundle : ScriptBundle
	{
		readonly IClientResourceInformationProvider informationProvider;
		readonly IClientResourceStreamProvider streamProvider;

		public ClientResourcesBundle( IClientResourceInformationProvider informationProvider, IClientResourceStreamProvider streamProvider,  string virtualPath = VirtualPathProvider.Application ) : base( virtualPath )
		{
			this.informationProvider = informationProvider;
			this.streamProvider = streamProvider;
		}

		public override IEnumerable<BundleFile> EnumerateFiles( BundleContext context )
		{
			try
			{
				var test = base.EnumerateFiles( context );
				Debugger.Break();
			}
			catch ( Exception e )
			{
				Console.WriteLine( e );
			}
			return Enumerable.Empty<BundleFile>();
			/*var result = this.Select( x => new BundleFile( x.VirtualPath, Activator.Create<ClientResourceFile>( x.VirtualPath ) ) ).ToArray();
			return result;#1#
			/*var manifests = ServiceLocation.Locate<IEnumerable<ClientResourceManifest>>() ?? Enumerable.Empty<ClientResourceManifest>();

			/*var files = AppDomain.CurrentDomain.GetAssemblies().Where( x => x.IsDecoratedWith<ClientResourcesAttribute>() ).SelectMany( x => x.GetAttributes<ClientResourceAttribute>().OrderByDescending( y => y.Priority )
					// .Select( y => new { Assembly = x, Attribute = y } ) )
					.Select( y => new EmbeddedResourceFile( x, y.FileName ) ) );
			var result = files.Select( x => new BundleFile( x.VirtualPath, x ) );
			return result;#2#
			return Enumerable.Empty<BundleFile>();#1#
		}
	}*/
}