using System;

namespace DragonSpark.Application.Runtime
{
	public readonly struct TransactionSpans<T>
	{
		public TransactionSpans(Memory<T> add, Memory<(T Stored, T Source)> update, Memory<T> delete)
		{
			Add    = add;
			Update = update;
			Delete = delete;
		}

		public Memory<T> Add { get; }

		public Memory<(T Stored, T Source)> Update { get; }

		public Memory<T> Delete { get; }

		public void Deconstruct(out Span<T> add, out Span<(T Stored, T Source)> update, out Span<T> delete)
		{
			add    = Add.Span;
			update = Update.Span;
			delete = Delete.Span;
		}
	}
}