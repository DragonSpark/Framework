using System;

namespace DragonSpark.Application.Model.Sequences;

public readonly record struct ActionSpans<TView, TModel>(Memory<TView> Add, Memory<Update<TView, TModel>> Update,
                                                         Memory<TModel> Delete)
{
	public void Deconstruct(out Span<TView> add, out Span<Update<TView, TModel>> update, out Span<TModel> delete)
	{
		add    = Add.Span;
		update = Update.Span;
		delete = Delete.Span;
	}
}