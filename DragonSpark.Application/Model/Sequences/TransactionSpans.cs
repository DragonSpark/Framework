using System;

namespace DragonSpark.Application.Model.Sequences;

public readonly struct TransactionSpans<T>(Memory<T> add, Memory<Update<T>> update, Memory<T> delete)
{
	public Memory<T> Add { get; } = add;

	public Memory<Update<T>> Update { get; } = update;

	public Memory<T> Delete { get; } = delete;

	public void Deconstruct(out Span<T> add, out Span<Update<T>> update, out Span<T> delete)
	{
		add    = Add.Span;
		update = Update.Span;
		delete = Delete.Span;
	}
}