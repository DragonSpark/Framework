using DragonSpark.Model.Sequences.Memory;
using System;

namespace DragonSpark.Application.Model.Sequences;

public readonly struct Actions<TModel, TView> : IDisposable
{
	public Actions(Leasing<TView> add, Leasing<Update<TModel, TView>> update, Leasing<TModel> delete)
	{
		Add    = add;
		Update = update;
		Delete = delete;
	}

	public Leasing<TView> Add { get; }

	public Leasing<Update<TModel, TView>> Update { get; }

	public Leasing<TModel> Delete { get; }

	public bool Any() => Add.Length > 0 || Update.Length > 0 || Delete.Length > 0;

	public ActionSpans<TModel, TView> AsSpans() => new(Add.AsMemory(), Update.AsMemory(), Delete.AsMemory());

	public ActionMemory<TModel, TView> AsMemory() => new(Add.AsMemory(), Update.AsMemory(), Delete.AsMemory());

	public void Deconstruct(out Leasing<TView> add, out Leasing<Update<TModel, TView>> update,
	                        out Leasing<TModel> delete)
	{
		add    = Add;
		update = Update;
		delete = Delete;
	}

	public void Dispose()
	{
		Add.Dispose();
		Update.Dispose();
		Delete.Dispose();
	}
}