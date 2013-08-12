using System.Web.Http;

namespace DragonSpark.Server.Configuration
{
	public class EnableQuerySupport : IHttpApplicationConfigurator
	{
		public void Configure( HttpConfiguration configuration )
		{
			configuration.EnableQuerySupport();
		}
	}
}
