using System;
using DragonSpark.Compose;
using DragonSpark.Reflection.Types;

namespace DragonSpark.Model.Selection.Stores
{
	sealed class Stores<TIn, TOut> : Select<Func<TIn, TOut>, ISelect<TIn, TOut>>
	{
		public static Stores<TIn, TOut> Default { get; } = new Stores<TIn, TOut>();

		Stores() : base(IsValueType.Default.Get(Type<TIn>.Metadata)
			                ? Selections<TIn, TOut>.Default
			                : Start.A.Generic(typeof(ReferenceTables<,>))
			                       .Of.Type<ISelect<Func<TIn, TOut>, ISelect<TIn, TOut>>>()
			                       .Get(typeof(TIn), typeof(TOut))) {}
	}
}