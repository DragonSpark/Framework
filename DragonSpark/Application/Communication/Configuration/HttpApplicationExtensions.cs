using System.Linq;
using System.Web;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Communication.Configuration
{
	public static class HttpApplicationExtensions
	{
		public static TModule GetModule<TModule>( this HttpApplication target ) where TModule : IHttpModule
		{
			var result = target.Modules.AllKeys.Select( y => target.Modules[ y ] ).FirstOrDefaultOfType<TModule>();
			return result;
		}
	}
}