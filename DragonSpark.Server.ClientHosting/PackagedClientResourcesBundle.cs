using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Optimization;

namespace DragonSpark.Server.ClientHosting
{
	/*public class PackagedClientResourcesBundle : ClientResourcesBundle
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
	}*/
}