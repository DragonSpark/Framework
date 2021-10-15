using DragonSpark.Model.Selection.Stores;
using System;

namespace DragonSpark.Model.Selection;

sealed class Selections<TIn, TOut> : ReferenceValueStore<Func<TIn, TOut>, ISelect<TIn, TOut>>
{
	public static Selections<TIn, TOut> Default { get; } = new Selections<TIn, TOut>();

	Selections() : base(x => x.Target as ISelect<TIn, TOut> ?? new Select<TIn, TOut>(x)) {}
}