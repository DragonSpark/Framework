using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;

namespace DragonSpark.Server
{
	public class ApplicationInitializer
	{
		readonly IServiceLocator locator;

		public ApplicationInitializer( IServiceLocator locator )
		{
			this.locator = locator;
		}

		public void Initialize( HttpApplication application, HttpConfiguration configuration )
		{
			AreaRegistration.RegisterAllAreas();

			new object[] { application, configuration }.Apply( locator.Register );

			var container = new IoCContainer( locator );
			configuration.DependencyResolver = container;
			DependencyResolver.SetResolver( locator );
			
			configuration.Services.Replace( typeof(IHttpControllerActivator), new CompositionRoot( locator ) );

			configuration.Formatters.Clear();

			configuration.Formatters.Add( new JsonNetFormatter() );
		}
	}
}