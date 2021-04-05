using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using JetBrains.Annotations;
using System.Collections.Concurrent;

namespace DragonSpark.Application.Runtime
{
	sealed class ScopedTable : DelegatedTable<string, object?>, IScopedTable
	{
		[UsedImplicitly]
		public ScopedTable() : this(new ConcurrentDictionary<string, object?>().ToTable()) {}

		public ScopedTable(ITable<string, object?> source) : base(source) {}
	}
}