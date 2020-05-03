using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Activation;
using System;
using System.Collections.Generic;

namespace DragonSpark.Model.Selection.Stores
{
	public class Lookup<TIn, TOut> : Conditional<TIn, TOut>,
	                                 IActivateUsing<IReadOnlyDictionary<TIn, TOut>>,
	                                 IActivateUsing<IDictionary<TIn, TOut>>
		where TIn : notnull
	{
		public Lookup(IDictionary<TIn, TOut> dictionary) : this(dictionary.AsReadOnly()) {}

		public Lookup(IReadOnlyDictionary<TIn, TOut> store) : this(store, Start.A.Selection<TIn>()
		                                                                       .By.Default<TOut>()) {}

		public Lookup(IReadOnlyDictionary<TIn, TOut> store, Func<TIn, TOut> @default)
			: base(store.ContainsKey, new TableValueAdapter<TIn, TOut>(store, @default).Get) {}
	}
}