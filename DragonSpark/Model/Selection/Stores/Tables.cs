using System;
using System.Collections.Concurrent;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Model.Selection.Stores
{
	public sealed class Tables<TIn, TOut> : Select<Func<TIn, TOut>, ITable<TIn, TOut>>
	{
		public static Tables<TIn, TOut> Default { get; } = new Tables<TIn, TOut>();

		Tables() : base(IsReference.Default.Get(Type<TIn>.Instance)
			                ? Start.A.Generic(typeof(ReferenceTables<,>))
			                       .Of.Type<ISelect<Func<TIn, TOut>, ITable<TIn, TOut>>>()
			                       .In(new Array<Type>(typeof(TIn), typeof(TOut)))
			                       .Assume()
			                       .Assume()
			                : Start.An.Instance(Activations<Func<TIn, TOut>, ConcurrentTables<TIn, TOut>>.Default)
			                       .Select(x => x.Get(new ConcurrentDictionary<TIn, TOut>()))) {}
	}
}