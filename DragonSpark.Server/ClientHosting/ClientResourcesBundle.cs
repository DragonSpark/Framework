using DragonSpark.Client;
using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Optimization;

namespace DragonSpark.Server.ClientHosting
{
	public class SignalRBundleFile : BundleFile
	{
		const string SignalRPath = "~/signalr/hubs";

		public SignalRBundleFile() : base( SignalRPath, new SignalRVirtualFile() )
		{}

		class SignalRVirtualFile : VirtualFile
		{
			public SignalRVirtualFile() : base( SignalRPath )
			{}

			public override Stream Open()
			{
				var url = new Uri( ServerContext.Current.Request.Url, VirtualPathUtility.ToAbsolute( SignalRPath ) );
				var result = new WebClient().OpenRead( url );
				return result;
			}
		}
	}

	public class PackagedClientResourcesBundle : ClientResourcesBundle
	{
		public PackagedClientResourcesBundle( string virtualPath = VirtualPathProvider.Application ) : base( virtualPath )
		{}

		public override IEnumerable<BundleFile> EnumerateFiles( BundleContext context )
		{
			Include( "~/signalr/hubs" );

			var enumerateFiles = base.EnumerateFiles( context ).Concat( new []
			{
				new SignalRBundleFile(),
				new BundleFile( VirtualPathProvider.Packaged, HostingEnvironment.VirtualPathProvider.GetFile( VirtualPathProvider.Packaged ) )
			} );
			return enumerateFiles;
		}
	}

	public class ClientResourcesBundle : ScriptBundle
	{
		public ClientResourcesBundle( string virtualPath = VirtualPathProvider.Directory ) : base( virtualPath )
		{}

		public override IEnumerable<BundleFile> EnumerateFiles( BundleContext context )
		{
			var files = AppDomain.CurrentDomain.GetAssemblies().Where( x => x.IsDecoratedWith<ClientResourcesAttribute>() ).SelectMany( x => x.GetAttributes<ClientResourceAttribute>().OrderByDescending( y => y.Priority )
					// .Select( y => new { Assembly = x, Attribute = y } ) )
					.Select( y => new EmbeddedResourceFile( x, y.FileName ) ) );
			var result = files.Select( x => new BundleFile( x.VirtualPath, x ) );
			return result;
		}
	}
}