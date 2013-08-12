using AttributeRouting.Web.Http.WebHost;
using DragonSpark.Server.App_Start;
using System.Web.Http;

[assembly: WebActivator.PreApplicationStartMethod(typeof(AttributeRoutingHttpConfig), "Start")]

namespace DragonSpark.Server.App_Start 
{
    public static class AttributeRoutingHttpConfig
	{
		public static void RegisterRoutes(HttpRouteCollection routes) 
		{    
			// See http://github.com/mccalltd/AttributeRouting/wiki for more options.
			// To debug routes locally using the built in ASP.NET development server, go to /routes.axd

            routes.MapHttpAttributeRoutes();
		}

        public static void Start() 
		{
            RegisterRoutes(GlobalConfiguration.Configuration.Routes);
        }
    }
}
