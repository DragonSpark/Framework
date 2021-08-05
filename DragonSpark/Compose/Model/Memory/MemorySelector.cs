using DragonSpark.Model.Sequences.Memory;
using System;
using System.Collections.Generic;

namespace DragonSpark.Compose.Model.Memory
{
	public readonly struct MemorySelector<T>
	{
		readonly Memory<T> _subject;

		public MemorySelector(Memory<T> subject) => _subject = subject;

		public Lease<T> Concat(Memory<T> memory) => ConcatenateMemory<T>.Default.Get(_subject, memory);

		public uint? IndexOf(T candidate) => IndexOf(candidate, EqualityComparer<T>.Default);

		public uint? IndexOf(T candidate, IEqualityComparer<T> comparer)
			=> IndexOf<T>.Default.Get(_subject, candidate, comparer);

		public bool Contains(T candidate) => Contains(candidate, EqualityComparer<T>.Default);

		public bool Contains(T candidate, IEqualityComparer<T> comparer)
			=> Contains<T>.Default.Get(_subject, candidate, comparer);

	}
}