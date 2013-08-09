using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;

namespace DragonSpark.Web
{
	public class ApplicationInitializer
	{
		readonly IServiceLocator locator;

		public ApplicationInitializer( IServiceLocator locator )
		{
			this.locator = locator;
		}

		public void Initialize( HttpConfiguration configuration )
		{
			AreaRegistration.RegisterAllAreas();

			configuration.DependencyResolver = new IoCContainer( locator );
			configuration.Services.Replace( typeof(IHttpControllerActivator), new CompositionRoot( locator ) );
			DependencyResolver.SetResolver(locator);

			configuration.Formatters.Where( f => f.SupportedMediaTypes.Any( v => v.MediaType.Equals( "application/json", StringComparison.CurrentCultureIgnoreCase ) ) ).ToArray().Apply( x => configuration.Formatters.Remove( x ) );

			var serializerSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
			serializerSettings.Converters.Add(new IsoDateTimeConverter());
			configuration.Formatters.Add(new JsonNetFormatter(serializerSettings));
		}
	}
}