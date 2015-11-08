using System.Collections.Generic;

namespace DragonSpark.Server.Configuration
{
	public class IgnoreRoute
	{
		public string Url { get; set; }

		public IDictionary<string, object> Constraints { get; set; }
	}
}