using System.Collections.Generic;
using DragonSpark.Objects;

namespace DragonSpark.Server.Configuration
{
	public class CustomAuthenticationClientRegistration
	{
		public IFactory ClientFactory { get; set; }

		public string DisplayName { get; set; }

		public IDictionary<string,object> ExtraData { get; set; }
	}
}