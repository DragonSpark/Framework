using DragonSpark.Extensions;
using DragonSpark.Io;
using DragonSpark.IoC;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;

namespace DragonSpark.Web
{
	[Singleton(typeof(IPathResolver))]
	public class PathResolver : IPathResolver
	{
		public string Resolve( string path )
		{
			var result = HostingEnvironment.MapPath( path );
			return result;
		}
	}

	public static class Network
	{
		public static IPAddress Determine( string address )
		{
			var data = address == "::1" ? "127.0.0.1" : address;
			var result = IPAddress.Parse( data );
			return result;
		}
	}

	public static class ControllerExtensions
	{
		public static IPAddress GetClientIpAddress( this Controller target )
		{
			var name = target.Request.ServerVariables["REMOTE_ADDR"];
			var result = Network.Determine( name );
			return result;
		}

		public static IPAddress GetClientIpAddress( this ApiController target )
		{
			var name = target.Request.Properties.TryGet( "MS_HttpContext" ).AsTo<HttpContextBase, string>( x => x.Request.UserHostAddress );
			var result = Network.Determine( name );
			return result;
		}
	}
}