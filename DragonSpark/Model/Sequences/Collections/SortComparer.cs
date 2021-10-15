namespace DragonSpark.Model.Sequences.Collections;

sealed class SortComparer<T> : DelegatedComparer<T>
{
	public static SortComparer<T> Default { get; } = new SortComparer<T>();

	SortComparer() : base(SortSelector<T>.Default.Get) {}
}