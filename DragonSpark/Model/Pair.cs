namespace DragonSpark.Model;

public readonly record struct Pair<TKey, TValue>(TKey Key, TValue Value)
{
	public static implicit operator (TKey, TValue)(Pair<TKey, TValue> instance) => (instance.Key, instance.Value);

	public static implicit operator Pair<TKey, TValue>((TKey, TValue) instance)
		=> Pairs.Create(instance.Item1, instance.Item2);

	public (TKey, TValue) Native() => (Key, Value);
}