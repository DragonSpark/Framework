using System;

namespace DragonSpark.Application.Model.Sequences;

public readonly struct TransactionSpans<T>
{
	public TransactionSpans(Memory<T> add, Memory<Update<T>> update, Memory<T> delete)
	{
		Add = add;
		Update = update;
		Delete = delete;
	}

	public Memory<T> Add { get; }

	public Memory<Update<T>> Update { get; }

	public Memory<T> Delete { get; }

	public void Deconstruct(out Span<T> add, out Span<Update<T>> update, out Span<T> delete)
	{
		add = Add.Span;
		update = Update.Span;
		delete = Delete.Span;
	}
}