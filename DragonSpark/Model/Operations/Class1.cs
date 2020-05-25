namespace DragonSpark.Model.Operations
{
	public interface IAssigning<TKey, TValue> : IResulting<(TKey Key, TValue Value)> {}

	public interface IDepending<in T> : ISelecting<T, bool> {}
}