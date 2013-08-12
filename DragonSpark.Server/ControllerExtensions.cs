using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using DragonSpark.Extensions;

namespace DragonSpark.Server
{
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