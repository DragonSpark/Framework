using DragonSpark.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Adapters;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Query;
using System;
using System.Collections.Generic;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static Query<_, T> Query<_, T>(this Selector<_, T[]> @this) => new Query<_, T>(@this.Get());

		public static Query<_, T> Query<_, T>(this Selector<_, Array<T>> @this) => @this.Get().Open().Query();

		public static Query<None, T> Query<T>(this IResult<Array<T>> @this) => @this.ToSelect().Query();

		public static Query<None, T> Query<T>(this IResult<IEnumerable<T>> @this) => @this.ToSelect().Query();

		public static Query<_, T> Query<_, T>(this ISelect<_, T[]> @this) => new Query<_, T>(@this);

		public static Query<_, T> Query<_, T>(this ISelect<_, Array<T>> @this) => @this.Open().Query();

		public static Query<_, T> Query<_, T>(this ISelect<_, IEnumerable<T>> @this)
			=> new Query<_, T>(@this.Select(Iterate<T>.Default));

		public static Query<_, T> Query<_, T>(this ISelect<_, ICollection<T>> @this)
			=> new Query<_, T>(@this.Select(CollectionSelection<T>.Default));

		public static Query<_, TOut> Select<_, T, TOut>(this Query<_, T> @this, ISelect<T, TOut> select)
			=> @this.Select(select.ToDelegate());

		public static SelectQueryContext<_, T> Select<_, T>(this Query<_, T> @this)
			=> new SelectQueryContext<_, T>(@this);

		public static Query<_, TOut> Select<_, T, TOut>(this Query<_, T> @this, Func<T, TOut> select)
			=> @this.Select(new Build.Select<T, TOut>(select).Returned());

		public static Query<_, TOut> SelectMany<_, T, TOut>(this Query<_, T> @this,
		                                                    ISelect<T, IEnumerable<TOut>> select)
			=> @this.SelectMany(select.Get);

		public static Query<_, TOut> SelectMany<_, T, TOut>(this Query<_, T> @this, Func<T, IEnumerable<TOut>> select)
			=> @this.Select(new Build.SelectMany<T, TOut>(select).Returned());

		public static WhereQueryContext<_, T> Where<_, T>(this Query<_, T> @this) => new WhereQueryContext<_, T>(@this);

		public static Query<_, T> Where<_, T>(this Query<_, T> @this, ICondition<T> where) => @this.Where(where.Get);

		public static ISelect<_, T> FirstAssigned<_, T>(this Query<_, T> @this) where T : class
			=> @this.Select(FirstAssigned<T>.Default);

		public static ISelect<_, T?> FirstAssigned<_, T>(this Query<_, T?> @this) where T : struct
			=> @this.Select(FirstAssignedValue<T>.Default);

		public static ISelect<_, int> Sum<_>(this Query<_, int> @this) => @this.Select(Sum32.Default);

		public static ISelect<_, uint> Sum<_>(this Query<_, uint> @this) => @this.Select(SumUnsigned32.Default);

		public static ISelect<_, long> Sum<_>(this Query<_, long> @this) => @this.Select(Sum64.Default);

		public static ISelect<_, ulong> Sum<_>(this Query<_, ulong> @this) => @this.Select(SumUnsigned64.Default);

		public static ISelect<_, float> Sum<_>(this Query<_, float> @this) => @this.Select(SumSingle.Default);

		public static ISelect<_, double> Sum<_>(this Query<_, double> @this) => @this.Select(SumDouble.Default);

		public static ISelect<_, decimal> Sum<_>(this Query<_, decimal> @this) => @this.Select(SumDecimal.Default);

		public static Func<TIn, TOut> Selector<TIn, TOut>(this Selector<TIn, TOut> @this) => @this;

		public static Query<_, T> Select<_, T>(this Query<_, T> @this, Selection selection)
			=> @this.Skip(selection.Start).Take(selection.Length);

		public static Query<_, T> Append<_, T>(this Query<_, T> @this, ISequence<T> others)
			=> @this.Select(new Build.Concatenation<T>(others).Returned());

		public static Query<_, T> Union<_, T>(this Query<_, T> @this, ISequence<T> others)
			=> @this.Union(others, EqualityComparer<T>.Default);

		public static Query<_, T> Union<_, T>(this Query<_, T> @this, ISequence<T> others,
											  IEqualityComparer<T> comparer)
			=> @this.Select(new Build.Union<T>(others, comparer).Returned());

		public static Query<_, T> Intersect<_, T>(this Query<_, T> @this, ISequence<T> others)
			=> @this.Intersect(others, EqualityComparer<T>.Default);

		public static Query<_, T> Intersect<_, T>(this Query<_, T> @this, ISequence<T> others,
												  IEqualityComparer<T> comparer)
			=> @this.Select(new Build.Intersect<T>(others, comparer).Returned());

		public static Query<_, T> Distinct<_, T>(this Query<_, T> @this) => @this.Select(Build.Distinct<T>.Default);

		public static Query<_, T> Distinct<_, T>(this Query<_, T> @this, IEqualityComparer<T> comparer)
			=> @this.Select(new Build.Distinct<T>(comparer));

		public static Query<_, T> Skip<_, T>(this Query<_, T> @this, uint count) => @this.Select(new Skip(count));

		public static Query<_, T> Take<_, T>(this Query<_, T> @this, uint count) => @this.Select(new Take(count));

		public static Query<_, T> Where<_, T>(this Query<_, T> @this, ISelect<T, bool> where) => @this.Where(where.Get);

		public static Query<_, T> Where<_, T>(this Query<_, T> @this, Func<T, bool> where)
			=> @this.Select(new Build.Where<T>(where));

		public static ISelect<_, IArrayMap<TKey, T>> GroupMap<_, T, TKey>(
			this Query<_, T> @this, ISelect<T, TKey> key) => @this.GroupMap(key, EqualityComparer<TKey>.Default);

		public static ISelect<_, IArrayMap<TKey, T>> GroupMap<_, T, TKey>(
			this Query<_, T> @this, ISelect<T, TKey> key, IEqualityComparer<TKey> comparer)
			=> @this.GroupMap(key.Get, comparer);

		public static ISelect<_, IArrayMap<TKey, T>> GroupMap<_, T, TKey>(this Query<_, T> @this, Func<T, TKey> key)
			=> @this.GroupMap(key, EqualityComparer<TKey>.Default);

		public static ISelect<_, IArrayMap<TKey, T>> GroupMap<_, T, TKey>(this Query<_, T> @this, Func<T, TKey> key,
																		  IEqualityComparer<TKey> comparer)
			=> @this.Select(new GroupMap<T, TKey>(key, comparer));

		public static ISelect<_, T> Only<_, T>(this Query<_, T> @this)
			=> @this.Select(Model.Sequences.Query.Only<T>.Default);

		public static ISelect<_, T> Only<_, T>(this Query<_, T> @this, Func<T, bool> where)
			=> @this.Select(new Only<T>(where));

		public static ISelect<_, T> FirstOrDefault<_, T>(this Query<_, T> @this)
			=> @this.Select(FirstOrDefault<T>.Default);

		public static ISelect<_, T> FirstOrDefault<_, T>(this Query<_, T> @this, Func<T, bool> where)
			=> @this.Select(new FirstWhere<T>(where));

		public static ISelect<_, T> Single<_, T>(this Query<_, T> @this) => @this.Select(Single<T>.Default);

		public static ISelect<_, T> Single<_, T>(this Query<_, T> @this, Func<T, bool> where)
			=> @this.Select(new Single<T>(where));

		public static ISelect<_, int> Sum<_, T>(this Query<_, T> @this, Func<T, int> select)
			=> @this.Select(new Sum32<T>(select));

		public static ISelect<_, uint> Sum<_, T>(this Query<_, T> @this, Func<T, uint> select)
			=> @this.Select(new SumUnsigned32<T>(select));

		public static ISelect<_, long> Sum<_, T>(this Query<_, T> @this, Func<T, long> select)
			=> @this.Select(new Sum64<T>(select));

		public static ISelect<_, ulong> Sum<_, T>(this Query<_, T> @this, Func<T, ulong> select)
			=> @this.Select(new SumUnsigned64<T>(select));

		public static ISelect<_, float> Sum<_, T>(this Query<_, T> @this, Func<T, float> select)
			=> @this.Select(new SumSingle<T>(select));

		public static ISelect<_, double> Sum<_, T>(this Query<_, T> @this, Func<T, double> select)
			=> @this.Select(new SumDouble<T>(select));

		public static ISelect<_, decimal> Sum<_, T>(this Query<_, T> @this, Func<T, decimal> select)
			=> @this.Select(new SumDecimal<T>(select));
	}
}