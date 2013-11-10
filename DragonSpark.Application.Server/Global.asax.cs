using System.Web;
using DragonSpark.Server;
using System.Web.Http;

namespace DragonSpark.Application.Server
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			var initializer = new ApplicationInitializer( new Configuration.Server().Instance ); 
			initializer.Initialize( this, GlobalConfiguration.Configuration );
			// BundleTable.EnableOptimizations = true;
		}
	}
}