using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Stores;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DragonSpark.Application.Connections
{
	public sealed class Sessions
		: DelegatedSelection<ConcurrentDictionary<string, List<UserConnection>>, ITable<string, List<UserConnection>>>
	{
		public static Sessions Default { get; } = new Sessions();

		Sessions() : this(new ConcurrentTables<string, List<UserConnection>>(_ => new List<UserConnection>())) {}

		public Sessions(ConcurrentTables<string, List<UserConnection>> tables)
			: base(tables.Get, () => new MappedConnections()) {}
	}
}