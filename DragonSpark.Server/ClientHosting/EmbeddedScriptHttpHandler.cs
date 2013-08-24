using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using DragonSpark.Extensions;

namespace DragonSpark.Server.ClientHosting
{
	public class EmbeddedScriptHttpHandler : IHttpHandler
	{
		readonly RouteData routeData;

		public EmbeddedScriptHttpHandler( RouteData routeData )
		{
			this.routeData = routeData;
		}

		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest( HttpContext context )
		{
			var routeDataValues = routeData.Values;
			var assemblyName = routeDataValues["assemblyName"].ToString();
			var filePath = routeDataValues["filePath"].ToString();
			var stream	= AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault( x => x.GetName().Name == assemblyName ).Transform( x => x.GetManifestResourceStream( filePath ) );
			stream.NotNull( x =>
			{
				context.Response.Clear();
				context.Response.ContentType = "text/javascript";
				stream.CopyTo( context.Response.OutputStream );
			} );
		}
	}
}