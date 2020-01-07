namespace DragonSpark.Model
{
	public static class Pairs
	{
		public static Pair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
			=> new Pair<TKey, TValue>(key, value);
	}
}