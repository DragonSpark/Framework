namespace DragonSpark.Model.Operations
{
	public interface IAssigning<TKey, TValue> : IResulting<(TKey Key, TValue Value)> {}
}