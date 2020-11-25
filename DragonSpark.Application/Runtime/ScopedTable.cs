using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using JetBrains.Annotations;
using System.Collections.Concurrent;

namespace DragonSpark.Application.Runtime
{
	public interface IScopedTable : ITable<string, object> {}

	sealed class ScopedTable : DelegatedTable<string, object>, IScopedTable
	{
		[UsedImplicitly]
		public ScopedTable() : this(new ConcurrentDictionary<string, object>().ToTable()) {}

		public ScopedTable(ITable<string, object> source) : base(source) {}
	}
}