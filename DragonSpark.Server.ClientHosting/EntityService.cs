using System.Collections.Generic;

namespace DragonSpark.Server.ClientHosting
{
	public class EntityService
	{
		public string Location { get; set; }

		public string InitializationMethod { get; set; }

		public IEnumerable<ClientModule> Extensions { get; set; }

		public IEnumerable<EntityQuery> Queries { get; set; }
	}
}