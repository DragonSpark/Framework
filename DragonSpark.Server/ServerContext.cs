using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System.Web;

namespace DragonSpark.Server
{
	public static class ServerContext
	{
		public static HttpContextBase Current
		{
			get
			{
				var wrapper = ServiceLocation.With<HttpApplication, HttpContext>( x => x.Context );
				var result = ( wrapper ?? HttpContext.Current ).Transform( x => new HttpContextWrapper( x ) );
				return result;
			}
		}
	}
}