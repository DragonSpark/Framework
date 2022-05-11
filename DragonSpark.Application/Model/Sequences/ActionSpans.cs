using System;

namespace DragonSpark.Application.Model.Sequences;

public readonly struct ActionSpans<TModel, TView>
{
	public ActionSpans(Memory<TView> add, Memory<Update<TModel, TView>> update, Memory<TModel> delete)
	{
		Add    = add;
		Update = update;
		Delete = delete;
	}

	public Memory<TView> Add { get; }

	public Memory<Update<TModel, TView>> Update { get; }

	public Memory<TModel> Delete { get; }

	public void Deconstruct(out Span<TView> add, out Span<Update<TModel, TView>> update, out Span<TModel> delete)
	{
		add    = Add.Span;
		update = Update.Span;
		delete = Delete.Span;
	}
}