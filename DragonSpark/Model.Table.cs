using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Runtime;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DragonSpark
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static IConditional<TIn, TOut> ToStore<TIn, TOut>(this IEnumerable<Pair<TIn, TOut>> @this)
			=> @this.ToOrderedDictionary().AsReadOnly().ToStore();

		public static ISelect<T, TIn, TOut> ToSelect<T, TIn, TOut>(this IEnumerable<Pair<T, Func<TIn, TOut>>> @this)
			=> new Select<T, TIn, TOut>(@this.ToStore().ToDelegate());

		public static IConditional<TIn, TOut> ToStore<TIn, TOut>(this IReadOnlyDictionary<TIn, TOut> @this)
			=> Compose.Start.An.Extent<Lookup<TIn, TOut>>().From(@this);

		public static ITable<TIn, TOut> ToTable<TIn, TOut>(this IDictionary<TIn, TOut> @this)
			=> StandardTables<TIn, TOut>.Default.Get(@this);

		public static ITable<TIn, TOut> ToTable<TIn, TOut>(this ConcurrentDictionary<TIn, TOut> @this)
			=> ConcurrentTables<TIn, TOut>.Default.Get(@this);

		public static ITable<TIn, TOut> ToTable<TIn, TOut>(this ConditionalWeakTable<TIn, TOut> @this)
			where TOut : class where TIn : class
			=> ReferenceValueTables<TIn, TOut>.Default.Get(@this);

		public static ITable<TIn, TOut> ToTable<TIn, TOut>(this ConditionalWeakTable<TIn, Tuple<TOut>> @this)
			where TIn : class where TOut : struct
			=> StructureValueTables<TIn, TOut>.Default.Get(@this);

		public static ITable<TIn, TOut> ToTable<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> @this.ToDelegateReference().ToTable();

		public static ITable<TIn, TOut> ToTable<TIn, TOut>(this Func<TIn, TOut> @this)
			=> Tables<TIn, TOut>.Default.Get(@this);

		public static ITable<TIn, TOut> ToStandardTable<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> @this.ToDelegateReference().ToStandardTable();

		public static ITable<TIn, TOut> ToStandardTable<TIn, TOut>(this ISelect<TIn, TOut> @this,
		                                                           IDictionary<TIn, TOut> table)
			=> @this.ToDelegateReference().ToStandardTable(table);

		public static ITable<TIn, TOut> ToStandardTable<TIn, TOut>(this Func<TIn, TOut> @this)
			=> @this.ToStandardTable(new Dictionary<TIn, TOut>());

		public static ITable<TIn, TOut> ToStandardTable<TIn, TOut>(this Func<TIn, TOut> @this,
		                                                           IDictionary<TIn, TOut> table)
			=> Compose.Start.An.Extent<StandardTables<TIn, TOut>>().From(@this).Get(table);

		public static ITable<TIn, TOut> ToConcurrentTable<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> @this.ToDelegateReference().ToConcurrentTable();

		public static ITable<TIn, TOut> ToConcurrentTable<TIn, TOut>(this ISelect<TIn, TOut> @this,
		                                                             ConcurrentDictionary<TIn, TOut> table)
			=> @this.ToDelegateReference().ToConcurrentTable(table);

		public static ITable<TIn, TOut> ToConcurrentTable<TIn, TOut>(this Func<TIn, TOut> @this)
			=> @this.ToConcurrentTable(new ConcurrentDictionary<TIn, TOut>());

		public static ITable<TIn, TOut> ToConcurrentTable<TIn, TOut>(this Func<TIn, TOut> @this,
		                                                             ConcurrentDictionary<TIn, TOut> table)
			=> Compose.Start.An.Extent<ConcurrentTables<TIn, TOut>>().From(@this).Get(table);
	}
}