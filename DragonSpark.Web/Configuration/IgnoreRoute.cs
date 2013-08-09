using System.Collections.Generic;

namespace DragonSpark.Web.Configuration
{
	public class IgnoreRoute
	{
		public string Url { get; set; }

		public IDictionary<string, object> Constraints { get; set; }
	}
}