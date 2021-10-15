using DragonSpark.Model.Selection.Stores;
using System;

namespace DragonSpark.Model.Selection;

sealed class Delegates<TIn, TOut> : ReferenceValueStore<ISelect<TIn, TOut>, Func<TIn, TOut>>
{
	public static Delegates<TIn, TOut> Default { get; } = new Delegates<TIn, TOut>();

	Delegates() : base(x => x.Get) {}
}