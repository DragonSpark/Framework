using System.Web.Http;

namespace DragonSpark.Web.Configuration
{
	public class EnableQuerySupport : IHttpApplicationConfigurator
	{
		public void Configure( HttpConfiguration configuration )
		{
			configuration.EnableQuerySupport();
		}
	}
}
