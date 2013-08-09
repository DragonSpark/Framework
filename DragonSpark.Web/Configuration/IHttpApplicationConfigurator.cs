using System.Web.Http;

namespace DragonSpark.Web.Configuration
{
	public interface IHttpApplicationConfigurator
	{
		void Configure( HttpConfiguration configuration );
	}
}