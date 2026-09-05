using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Application.Runtime;

sealed class Sources<T> : ConcurrentTable<T, CancellationTokenSource> where T : notnull
{
	public Sources() : base(_ => new()) {}
}