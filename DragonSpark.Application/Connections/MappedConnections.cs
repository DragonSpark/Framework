using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DragonSpark.Application.Connections
{
	sealed class MappedConnections : ConcurrentDictionary<string, List<UserConnection>>
	{
		public MappedConnections() : base(StringComparer.InvariantCultureIgnoreCase) {}
	}
}