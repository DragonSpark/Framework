using DragonSpark.Compose;
using DragonSpark.Compose.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using FluentAssertions;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Enumerable = System.Linq.Enumerable;

namespace DragonSpark.Testing.Application.Compose.Model
{
	public sealed class SequentialQueryTests
	{
		[Fact]
		void Verify()
		{
			var input = Enumerable.ToArray(Enumerable.Range(0, 100));
			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Open.By.Self.ToQuery()
			     .Where(x => x > 50)
			     .Get(input)
			     .Open()
			     .Should()
			     .HaveCount(49)
			     .And.Subject.All(x => x > 50)
			     .Should()
			     .BeTrue();
		}
	}

	public static class Extensions
	{
		public static OpenSequentialQuery<_, T> ToQuery<_, T>(this Selector<_, T[]> @this)
			=> new OpenSequentialQuery<_, T>(@this);

		/*[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] ToLeasedArray<TEnumerable, TEnumerator, T>(this TEnumerable source)
			where TEnumerable : IValueReadOnlyList<T, TEnumerator>
			where TEnumerator : struct, IEnumerator<T>
			=> source.ToLeasedArray<TEnumerable, TEnumerator, T>(ArrayPool<T>.Shared);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Store<T> ToLeasedArray<TEnumerable, TEnumerator, T>(
			this TEnumerable source, ArrayPool<T> pool)
			where TEnumerable : IValueReadOnlyList<T, TEnumerator>
			where TEnumerator : struct, IEnumerator<T>
		{
			var count  = source.Count;
			var result = pool.Rent(count);
			for (var index = 0; index < count; ++index)
			{
				result[index] = source[index];
			}

			return result;
		}*/
	}

	public interface IQuery<in TIn, T, out TOut, V> : ISelect<TIn, TOut>
		where TIn : IEnumerable<T>
		where TOut : IEnumerable<V> {}

	public interface IReferenceQuery<TIn, TOut> : IQuery<IEnumerable<TIn>, TIn, IEnumerable<TOut>, TOut> {}

	public interface IOpenQuery<TIn, TOut> : IQuery<TIn[], TIn, IEnumerable<TOut>, TOut> {}

	public interface IStructureQuery<in TInEnumerable, TIn, TInEnumerator, out TOutEnumerable, TOut, TOutEnumerator>
		: IQuery<TInEnumerable, TIn, TOutEnumerable, TOut>
		where TInEnumerable : IValueReadOnlyList<TIn, TInEnumerator>
		where TInEnumerator : struct, IEnumerator<TIn>
		where TOutEnumerable : IValueReadOnlyList<TOut, TOutEnumerator>
		where TOutEnumerator : struct, IEnumerator<TOut> {}

	sealed class StructureQuery<TInEnumerable, TIn, TInEnumerator, TOutEnumerable, TOut, TOutEnumerator>
		: Select<TInEnumerable, TOutEnumerable>,
		  IStructureQuery<TInEnumerable, TIn, TInEnumerator, TOutEnumerable, TOut, TOutEnumerator>
		where TInEnumerable : IValueReadOnlyList<TIn, TInEnumerator>
		where TInEnumerator : struct, IEnumerator<TIn>
		where TOutEnumerable : IValueReadOnlyList<TOut, TOutEnumerator>
		where TOutEnumerator : struct, IEnumerator<TOut>
	{
		public StructureQuery(ISelect<TInEnumerable, TOutEnumerable> select) : base(select) {}

		public StructureQuery(Func<TInEnumerable, TOutEnumerable> select) : base(select) {}
	}

	public interface IContinuation<out _, in TInQueryEnumerable, T,
	                               out TOutQuery, TOutQueryEnumerable, V>
		: ISelect<ISelect<_, TInQueryEnumerable>, TOutQuery>
		where TInQueryEnumerable : IEnumerable<T>
		where TOutQuery : SequentialQuery<_, TOutQueryEnumerable, V>
		where TOutQueryEnumerable : IEnumerable<V> {}

	public class OpenContinuation<_, T, TOutQuery, TOutQueryEnumerable, V>
		: Continuation<_, T[], T, TOutQuery, TOutQueryEnumerable, V>
		where TOutQuery : SequentialQuery<_, TOutQueryEnumerable, V>
		where TOutQueryEnumerable : IEnumerable<V>
	{
		public OpenContinuation(Func<ISelect<_, T[]>, TOutQuery> select) : base(select) {}
	}

	public sealed class Where<_, T> : ISelect<ISelect<_, T[]>, ReferenceSequentialQuery<_, T>>
	{
		readonly Func<T, bool> _where;

		public Where(Func<T, bool> where) => _where = @where;

		public ReferenceSequentialQuery<_, T> Get(ISelect<_, T[]> parameter)
			=> new ReferenceSequentialQuery<_, T>(parameter.Select(x => x.AsEnumerable().Where(_where)).Then());
	}

	public sealed class OpenSequenceContinuation<_, TIn, TOut>
		: OpenContinuation<_, TIn, ReferenceSequentialQuery<_, TOut>, IEnumerable<TOut>, TOut>
	{
		public OpenSequenceContinuation(Func<ISelect<_, TIn[]>, ReferenceSequentialQuery<_, TOut>> select)
			: base(select) {}
	}

	public class Continuation<_, TInQueryEnumerable, T,
	                          TOutQuery, TOutQueryEnumerable, V>
		: IContinuation<_, TInQueryEnumerable, T,
			TOutQuery, TOutQueryEnumerable, V>
		where TInQueryEnumerable : IEnumerable<T>
		where TOutQuery : SequentialQuery<_, TOutQueryEnumerable, V>
		where TOutQueryEnumerable : IEnumerable<V>
	{
		readonly Func<ISelect<_, TInQueryEnumerable>, TOutQuery> _select;

		public Continuation(Func<ISelect<_, TInQueryEnumerable>, TOutQuery> select) => _select = select;

		public TOutQuery Get(ISelect<_, TInQueryEnumerable> parameter) => _select(parameter);
	}

	public sealed class OpenSequentialQuery<_, T> : SequentialQuery<_, T[], T>
	{
		readonly Selector<_, T[]> _selector;

		public OpenSequentialQuery(Selector<_, T[]> selector) : base(selector, x => x) => _selector = selector;

		public ReferenceSequentialQuery<_, T> Where(Func<T, bool> where)
			=> Query(new OpenSequenceContinuation<_, T, T>(new Where<_, T>(where).Get));

		/*public SequentialQuery<_, TOut, V> Query<TOut, V>(Func<T[], TOut> select) where TOut : IEnumerable<V>
			=> default;*/
	}

	public sealed class ReferenceSequentialQuery<_, T> : SequentialQuery<_, IEnumerable<T>, T>
	{
		public ReferenceSequentialQuery(Selector<_, IEnumerable<T>> selector) : base(selector, x => x.ToArray()) {}
	}

	public class SequentialQuery<_, TEnumerable, T> : IResult<ISelect<_, Array<T>>> where TEnumerable : IEnumerable<T>
	{
		public static implicit operator Func<_, Array<T>>(SequentialQuery<_, TEnumerable, T> instance)
			=> instance.Get().Get;

		readonly Selector<_, TEnumerable> _selector;
		readonly Func<TEnumerable, T[]>   _open;

		public SequentialQuery(Selector<_, TEnumerable> selector, Func<TEnumerable, T[]> open)
		{
			_selector = selector;
			_open     = open;
		}

		public TOutQuery Query<TOutQuery, TOutQueryEnumerable, V>(
			IContinuation<_, TEnumerable, T, TOutQuery, TOutQueryEnumerable, V> query)
			where TOutQuery : SequentialQuery<_, TOutQueryEnumerable, V>
			where TOutQueryEnumerable : IEnumerable<V>
			=> query.Get(_selector.Get());

		/*public SequentialQuery<_, TOut, V> Query<TOut, V>(Func<TEnumerable, TOut> select) where TOut : IEnumerable<V>
			=> default;*/

		/*public MaterializedQuery<_, TOut> Select<TOut, V>(Func<T, V> select)
			=> default;*/

		public ISelect<_, Array<T>> Get() => _selector.Select(_open).Select(x => x.Result()).Get();
	}
}