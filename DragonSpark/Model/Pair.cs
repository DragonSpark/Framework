﻿namespace DragonSpark.Model
{
	public readonly struct Pair<TKey, TValue>
	{
		public Pair(TKey key, TValue value)
		{
			Key   = key;
			Value = value;
		}

		public TKey Key { get; }

		public TValue Value { get; }

		public void Deconstruct(out TKey key, out TValue value)
		{
			key   = Key;
			value = Value;
		}
	}
}