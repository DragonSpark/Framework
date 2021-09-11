using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DragonSpark.Application.Connections
{
	public sealed class Sessions
		: DelegatedSelection<ConcurrentDictionary<string, List<UserConnection>>, ITable<string, List<UserConnection>>>
	{
		public static Sessions Default { get; } = new Sessions();

		Sessions() : this(x => new ConcurrentTable<string, List<UserConnection>>(x, _ => new List<UserConnection>())) {}

		public Sessions(Func<ConcurrentDictionary<string, List<UserConnection>>, ITable<string, List<UserConnection>>> tables)
			: base(tables, () => new MappedConnections()) {}
	}
}