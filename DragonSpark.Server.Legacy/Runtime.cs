using System.Diagnostics;
using System.Web;
using DragonSpark.Extensions;

namespace DragonSpark.Server.Legacy
{
	public static class Runtime
	{
		public static bool IsInLocalDevelopmentEnvironment()
		{
			var result = Debugger.IsAttached || HttpContext.Current.Transform( x => x.Request.Url.Transform( y => y.Host ).Transform( y => y.Contains( "::1" ) || y.Contains( "localhost" ) || y.Contains( "127.0.0.1" ) ) );
			return result;
		}
	}
}