namespace DragonSpark.Model
{
	public readonly struct Pair<TKey, TValue>
	{
		public static implicit operator (TKey, TValue)(Pair<TKey, TValue> instance) => (instance.Key, instance.Value);

		public static implicit operator Pair<TKey, TValue>((TKey, TValue) instance)
			=> Pairs.Create(instance.Item1, instance.Item2);

		public Pair(TKey key, TValue value)
		{
			Key   = key;
			Value = value;
		}

		public TKey Key { get; }

		public TValue Value { get; }

		public (TKey, TValue) Native() => (Key, Value);

		public void Deconstruct(out TKey key, out TValue value)
		{
			key   = Key;
			value = Value;
		}
	}
}