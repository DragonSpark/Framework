using DragonSpark.Application.Configuration;
using DragonSpark.Web;
using System.Web.Http;

namespace DragonSpark.Application
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			var initializer = new ApplicationInitializer( new Server().Instance ); 
			initializer.Initialize( GlobalConfiguration.Configuration );
		}
	}
}