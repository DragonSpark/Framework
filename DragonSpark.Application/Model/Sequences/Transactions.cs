using DragonSpark.Model.Sequences.Memory;
using System;

namespace DragonSpark.Application.Model.Sequences;

public readonly struct Transactions<T> : IDisposable
{
	public Transactions(Leasing<T> add, Leasing<Update<T>> update, Leasing<T> delete)
	{
		Add = add;
		Update = update;
		Delete = delete;
	}

	public Leasing<T> Add { get; }

	public Leasing<Update<T>> Update { get; }

	public Leasing<T> Delete { get; }

	public bool Any() => Add.Length > 0 || Update.Length > 0 || Delete.Length > 0;

	public TransactionSpans<T> AsSpans() => new(Add.AsMemory(), Update.AsMemory(), Delete.AsMemory());

	public void Deconstruct(out Memory<T> add, out Memory<Update<T>> update, out Memory<T> delete)
	{
		add = Add.AsMemory();
		update = Update.AsMemory();
		delete = Delete.AsMemory();
	}

	public void Dispose()
	{
		Add.Dispose();
		Update.Dispose();
		Delete.Dispose();
	}
}