using DragonSpark.Model.Sequences.Memory;
using System;

namespace DragonSpark.Compose.Model.Memory
{
	public readonly struct MemorySelector<T>
	{
		readonly Memory<T> _subject;

		public MemorySelector(Memory<T> subject) => _subject = subject;

		public Lease<T> Concat(Memory<T> memory) => Concatenate<T>.Default.Get(_subject, memory);
	}
}