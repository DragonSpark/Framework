using DragonSpark.Extensions;
using DragonSpark.Io;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;

namespace DragonSpark.Server.ClientHosting
{
	public class EmbeddedScriptHttpHandler : IHttpHandler
	{
		readonly IPathResolver pathResolver;
		readonly RouteData routeData;

		public EmbeddedScriptHttpHandler( IPathResolver pathResolver, RouteData routeData )
		{
			this.pathResolver = pathResolver;
			this.routeData = routeData;
		}

		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest( HttpContext context )
		{
			var routeDataValues = routeData.Values;

			var assemblyName = routeDataValues[ "assemblyName" ].ToString();
			var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault( x => x.GetName().Name == assemblyName );

			var filePath = routeDataValues[ "filePath" ].ToString();

			var stream = DetermineLocal( assembly, filePath ) ?? assembly.Transform( x => new[] { filePath, string.Concat( assemblyName, ".", filePath ).Replace( Path.AltDirectorySeparatorChar, '.' ) }.Select( x.GetManifestResourceStream ).NotNull().FirstOrDefault() );
			stream.NotNull( x =>
			{
				context.Response.Clear();
				context.Response.ContentType = "text/javascript";
				x.CopyTo( context.Response.OutputStream );
			} );
		}

		Stream DetermineLocal( Assembly assembly, string filePath )
		{
			try
			{
				var path = Path.Combine( pathResolver.Resolve( "~/" ), "..", assembly.GetName().Name, filePath );
				var result = File.Exists( path ) ? new MemoryStream( File.ReadAllBytes( path ) ) : null;
				return result;
			}
			catch ( IOException )
			{
				return null;
			}
		}
	}
}