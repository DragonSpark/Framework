using System.Collections.Generic;
using System.Web.Http;

namespace DragonSpark.Server.Configuration
{
	public abstract class RegisterClientBase  : IHttpApplicationConfigurator
	{
		public string DisplayName { get; set; }

		public IDictionary<string,object> ExtraData { get; set; }


		public abstract void Configure( HttpConfiguration configuration );
	}
}