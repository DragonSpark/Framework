using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DragonSpark.Application.Connections;

sealed class Stores : Select<ConcurrentDictionary<string, List<UserConnection>>, ITable<string, List<UserConnection>>>
{
	public static Stores Default { get; } = new();

	Stores() : base(x => new ConcurrentTable<string, List<UserConnection>>(x, _ => new List<UserConnection>())) {}
}