using System.Collections.Generic;

namespace DragonSpark.Server.ClientHosting
{
	public class ServerConfiguration
	{
		public IEnumerable<string> Hubs { get; set; }

		public IEnumerable<EntityService> EntityServices { get; set; }
	}
}