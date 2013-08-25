using DragonSpark.Extensions;
using DragonSpark.Io;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;

namespace DragonSpark.Server.ClientHosting
{
	public class ClientResourceHttpHandler : IHttpHandler
	{
		readonly IPathResolver pathResolver;
		readonly RouteData routeData;

		public ClientResourceHttpHandler( IPathResolver pathResolver, RouteData routeData )
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

		static string DeterminePath( string root, string filePath, string name )
		{
			var result = Path.Combine( root, filePath );
			if ( !File.Exists( result ) )
			{
				var directory = root;
				var queue = new Queue<string>( filePath.Replace( name, string.Empty ).ToStringArray( '.' ) );
				while ( queue.Any() )
				{
					var file = Path.Combine( directory, string.Join( ".", queue.ToArray() ) );
					if ( File.Exists( file ) )
					{
						return file;
					}
					directory = Path.Combine( directory, queue.Dequeue() );
				}
				return null;
			}
			return result;
		}

		Stream DetermineLocal( Assembly assembly, string filePath )
		{
			try
			{
				var name = assembly.GetName().Name;
				var root = Path.Combine( pathResolver.Resolve( "~/" ), "..", name );

				var path = DeterminePath( root, filePath, name );

				var result = path.Transform( x => new MemoryStream( File.ReadAllBytes( x ) ) );
				return result;
			}
			catch ( IOException )
			{
				return null;
			}
		}
	}
}