using DragonSpark.Model;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static IConditional<TIn, TOut> ToStore<TIn, TOut>(this IEnumerable<Pair<TIn, TOut>> @this)
			where TIn : notnull
			=> @this.ToOrderedDictionary().AsReadOnly().ToStore();

		public static ISelect<T, TIn, TOut> ToSelect<T, TIn, TOut>(this IEnumerable<Pair<T, Func<TIn, TOut>>> @this)
			where T : notnull
			where TIn : notnull
			=> new Select<T, TIn, TOut>(@this.ToStore().Get);

		public static IConditional<TIn, TOut> ToStore<TIn, TOut>(this IReadOnlyDictionary<TIn, TOut> @this)
			where TIn : notnull
		// ReSharper disable once RedundantNameQualifier
			=> Compose.Start.An.Extent<DragonSpark.Model.Selection.Stores.Lookup<TIn, TOut>>().From(@this);

		public static ITable<TIn, TOut> ToTable<TIn, TOut>(this IDictionary<TIn, TOut> @this)
			where TIn : notnull
			=> StandardTables<TIn, TOut>.Default.Get(@this);

		public static ITable<TIn, TOut> ToTable<TIn, TOut>(this ConcurrentDictionary<TIn, TOut> @this)
			where TIn : notnull
			=> ConcurrentTables<TIn, TOut>.Default.Get(@this);

		public static ITable<TIn, TOut> ToTable<TIn, TOut>(this ConditionalWeakTable<TIn, TOut> @this)
			where TOut : class where TIn : class
			=> ReferenceValueTables<TIn, TOut>.Default.Get(@this);

		public static ITable<TIn, TOut> ToTable<TIn, TOut>(this ConditionalWeakTable<TIn, Tuple<TOut>> @this)
			where TIn : class where TOut : struct
			=> StructureValueTables<TIn, TOut>.Default.Get(@this);

		public static ITable<TIn, TOut> ToTable<TIn, TOut>(this ISelect<TIn, TOut> @this)
			where TIn : notnull
			=> @this.ToDelegateReference().ToTable();

		public static ITable<TIn, TOut> ToTable<TIn, TOut>(this Func<TIn, TOut> @this)
			where TIn : notnull
			=> Tables<TIn, TOut>.Default.Get(@this);

		public static ITable<TIn, TOut> ToStandardTable<TIn, TOut>(this ISelect<TIn, TOut> @this)
			where TIn : notnull
			=> @this.ToDelegateReference().ToStandardTable();

		public static ITable<TIn, TOut> ToStandardTable<TIn, TOut>(this ISelect<TIn, TOut> @this,
		                                                           IDictionary<TIn, TOut> table)
			where TIn : notnull
			=> @this.ToDelegateReference().ToStandardTable(table);

		public static ITable<TIn, TOut> ToStandardTable<TIn, TOut>(this Func<TIn, TOut> @this)
			where TIn : notnull
			=> @this.ToStandardTable(new Dictionary<TIn, TOut>());

		public static ITable<TIn, TOut> ToStandardTable<TIn, TOut>(this Func<TIn, TOut> @this,
		                                                           IDictionary<TIn, TOut> table)
			where TIn : notnull
			=> Compose.Start.An.Extent<StandardTables<TIn, TOut>>().From(@this).Get(table);

		public static ITable<TIn, TOut> ToConcurrentTable<TIn, TOut>(this ISelect<TIn, TOut> @this)
			where TIn : notnull
			=> @this.ToDelegateReference().ToConcurrentTable();

		public static ITable<TIn, TOut> ToConcurrentTable<TIn, TOut>(this ISelect<TIn, TOut> @this,
		                                                             ConcurrentDictionary<TIn, TOut> table)
			where TIn : notnull
			=> @this.ToDelegateReference().ToConcurrentTable(table);

		public static ITable<TIn, TOut> ToConcurrentTable<TIn, TOut>(this Func<TIn, TOut> @this)
			where TIn : notnull
			=> @this.ToConcurrentTable(new ConcurrentDictionary<TIn, TOut>());

		public static ITable<TIn, TOut> ToConcurrentTable<TIn, TOut>(this Func<TIn, TOut> @this,
		                                                             ConcurrentDictionary<TIn, TOut> table)
			where TIn : notnull
			=> Compose.Start.An.Extent<ConcurrentTables<TIn, TOut>>().From(@this).Get(table);

		public static bool TryPop<TIn, TOut>(this ITable<TIn, TOut> @this, TIn key, out TOut element)
		{
			var result = @this.IsSatisfiedBy(key);
			element = result ? @this.Get(key) : default!;
			return result ? @this.Remove(key) : result;
		}

		public static bool TryGet<TIn, TOut>(this IConditional<TIn, TOut> @this, TIn key, out TOut element)
		{
			var result = @this.IsSatisfiedBy(key);
			element = result ? @this.Get(key) : default!;
			return result;
		}
	}
}