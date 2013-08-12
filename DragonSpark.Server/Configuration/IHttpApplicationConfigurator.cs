using System.Web.Http;

namespace DragonSpark.Server.Configuration
{
	public interface IHttpApplicationConfigurator
	{
		void Configure( HttpConfiguration configuration );
	}
}