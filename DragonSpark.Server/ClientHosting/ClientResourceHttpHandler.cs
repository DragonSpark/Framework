using DragonSpark.Client;
using DragonSpark.Extensions;
using DragonSpark.Io;
using DragonSpark.Logging;
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
		readonly string frameworkPath;

		public ClientResourceHttpHandler( IPathResolver pathResolver, RouteData routeData, string frameworkPath )
		{
			this.pathResolver = pathResolver;
			this.routeData = routeData;
			this.frameworkPath = frameworkPath;
		}

		public bool IsReusable
		{
			get { return false; }
		}

		void IHttpHandler.ProcessRequest( HttpContext context )
		{
			var routeDataValues = routeData.Values;

			var assemblyName = routeDataValues[ "assemblyName" ].ToString();
			var assembly = AppDomain.CurrentDomain.GetAssemblies().Where( x => x.IsDecoratedWith<ClientResourcesAttribute>() ).FirstOrDefault( x => x.GetName().Name == assemblyName );
			if ( assembly != null )
			{
				var filePath = routeDataValues["filePath"].ToString();
				Log.Information( string.Format( "File Path = {0}", filePath ) );
				var stream = DetermineLocal( assembly, filePath ) ?? new EmbeddedResourceFile( assembly, filePath ).Open();
				if ( stream != null )
				{
					context.Response.Clear();
					context.Response.ContentType = filePath.EndsWith( ".js" ) ? "text/javascript" : MimeMapping.GetMimeMapping( filePath );
					stream.CopyTo( context.Response.OutputStream );
				}
				else
				{
					Log.Warning( string.Format( "Could not load stream for {0}.", filePath ) );
				}
			}
			else
			{
				Log.Warning( string.Format( "Assembly not found for: {0}", assemblyName ) );
			}
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

				var candidates = new[] { frameworkPath, ".." };

				var result = candidates.Select( x => Path.Combine( pathResolver.Resolve( "~/" ), x, name ).Transform( y => DeterminePath( y, filePath, name ).Transform( z => new MemoryStream( File.ReadAllBytes( z ) ) ) ) ).NotNull().FirstOrDefault();

				return result;
			}
			catch ( IOException )
			{
				return null;
			}	
		}
	}
}