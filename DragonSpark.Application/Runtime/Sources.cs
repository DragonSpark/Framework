using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Application.Runtime;

sealed class Sources<T> : ConcurrentTable<T, CancellationTokenSource> where T : notnull
{
	public static Sources<T> Default { get; } = new();

	Sources() : base(_ => new()) {}
}