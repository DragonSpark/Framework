using DragonSpark.Compose;
using DragonSpark.Reflection;
using DragonSpark.Runtime.Activation;
using System;
using System.Collections.Concurrent;
using Array = DragonSpark.Model.Sequences.Array;

namespace DragonSpark.Model.Selection.Stores
{
	public sealed class Tables<TIn, TOut> : Select<Func<TIn, TOut>, ITable<TIn, TOut>> where TIn : notnull
	{
		public static Tables<TIn, TOut> Default { get; } = new Tables<TIn, TOut>();

		Tables() : base(IsReference.Default.Get(A.Type<TIn>())
			                ? Start.A.Generic(typeof(ReferenceTables<,>))
			                       .Of.Type<ISelect<Func<TIn, TOut>, ITable<TIn, TOut>>>()
			                       .Then()
			                       .Bind(Array.Of(A.Type<TIn>(), A.Type<TOut>()))
			                       .Get()
			                       .Then()
			                       .Assume()
			                       .Instance()
			                : Start.An.Instance(Activations<Func<TIn, TOut>, ConcurrentTables<TIn, TOut>>.Default)
			                       .Select(x => x.Get(new ConcurrentDictionary<TIn, TOut>()))) {}
	}
}