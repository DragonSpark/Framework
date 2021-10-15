using DragonSpark.Compose;
using DragonSpark.Reflection.Types;
using System;

namespace DragonSpark.Model.Selection.Stores;

sealed class Stores<TIn, TOut> : Select<Func<TIn, TOut>, ISelect<TIn, TOut>>
{
	public static Stores<TIn, TOut> Default { get; } = new Stores<TIn, TOut>();

	Stores() : base(IsValueType.Default.Get(A.Metadata<TIn>())
		                ? Selections<TIn, TOut>.Default
		                : Start.A.Generic(typeof(ReferenceTables<,>))
		                       .Of.Type<ISelect<Func<TIn, TOut>, ISelect<TIn, TOut>>>()
		                       .Get(A.Type<TIn>(), A.Type<TOut>())) {}
}