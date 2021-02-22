using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DragonSpark.Application.Connections
{
	public sealed class Identifiers
		: DelegatedSelection<ConcurrentDictionary<string, List<UserConnection>>, ITable<string, List<UserConnection>>>
	{
		public static Identifiers Default { get; } = new Identifiers();

		Identifiers()
			: base(new ConcurrentTables<string, List<UserConnection>>(_ => new List<UserConnection>()).Get,
			       () => new ConcurrentDictionary<string, List<UserConnection>>(StringComparer
				                                                                    .InvariantCultureIgnoreCase)) {}
	}
}