using DragonSpark.Server;
using System.Web.Http;

namespace DragonSpark.Application.Server
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			var initializer = new ApplicationInitializer( new Configuration.Server().Instance ); 
			initializer.Initialize( GlobalConfiguration.Configuration );
			// RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
		}
	}
}