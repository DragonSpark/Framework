using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using Array = NetFabric.Hyperlinq.Array;

namespace DragonSpark.Compose.Model
{
	public interface IOpenQuery<TIn, out TOut> : ISelect<Array.ValueEnumerableWrapper<TIn>, TOut[]> {}

	sealed class OpenQuery<TIn, TOut> : Select<Array.ValueEnumerableWrapper<TIn>, TOut[]>, IOpenQuery<TIn, TOut>
	{
		public OpenQuery(Func<Array.ValueEnumerableWrapper<TIn>, TOut[]> select) : base(select) {}
	}

	public interface IOpenReduce<TIn, out TOut> : ISelect<Array.ValueEnumerableWrapper<TIn>, TOut> {}

	sealed class OpenReduce<TIn, TOut> : Select<Array.ValueEnumerableWrapper<TIn>, TOut>, IOpenReduce<TIn, TOut>
	{
		public OpenReduce(Func<Array.ValueEnumerableWrapper<TIn>, TOut> select) : base(select) {}
	}

	public interface ISequenceQuery<TIn, out TOut> : ISelect<Enumerable.ValueEnumerableWrapper<TIn>, TOut[]> {}

	sealed class SequenceQuery<TIn, TOut> : Select<Enumerable.ValueEnumerableWrapper<TIn>, TOut[]>,
	                                        ISequenceQuery<TIn, TOut>
	{
		public SequenceQuery(Func<Enumerable.ValueEnumerableWrapper<TIn>, TOut[]> select) : base(select) {}
	}

	public interface ISequenceReduce<TIn, out TOut> : ISelect<Enumerable.ValueEnumerableWrapper<TIn>, TOut> {}

	sealed class SequenceReduce<TIn, TOut> : Select<Enumerable.ValueEnumerableWrapper<TIn>, TOut>,
	                                         ISequenceReduce<TIn, TOut>
	{
		public SequenceReduce(Func<Enumerable.ValueEnumerableWrapper<TIn>, TOut> select) : base(select) {}
	}

	public interface IQuerySelector<in _, T> : IResult<ISelect<_, Array<T>>>
	{

	}

	// TODO: name
	public sealed class SequenceQuerySelector<_, T> : IResult<ISelect<_, Array<T>>>
	{
		public static implicit operator Func<_, Array<T>>(SequenceQuerySelector<_, T> instance) => instance.Get().Get;

		readonly Selector<_, IEnumerable<T>> _selector;

		public SequenceQuerySelector(Selector<_, IEnumerable<T>> selector) => _selector = selector;

		public OpenQuerySelector<_, TOut> Select<TOut>(Func<Enumerable.ValueEnumerableWrapper<T>, TOut[]> select)
			=> Select(new SequenceQuery<T, TOut>(@select));

		public OpenQuerySelector<_, TOut> Select<TOut>(ISequenceQuery<T, TOut> query)
			=> new OpenQuerySelector<_, TOut>(_selector.Select(x => x.AsValueEnumerable()).Select(query));

		public Selector<_, TOut> Reduce<TOut>(Func<Enumerable.ValueEnumerableWrapper<T>, TOut> select)
			=> Reduce(new SequenceReduce<T, TOut>(@select));

		public Selector<_, TOut> Reduce<TOut>(ISequenceReduce<T, TOut> query)
			=> _selector.Select(x => x.AsValueEnumerable()).Select(query);

		public ISelect<_, T[]> Out() => _selector.Get().Open();

		public ISelect<_, Array<T>> Get() => _selector.Select(DragonSpark.Model.Sequences.Result<T>.Default).Get();
	}

	// TODO: name
	public sealed class OpenQuerySelector<_, T> : IResult<ISelect<_, Array<T>>>
	{
		public static implicit operator Func<_, Array<T>>(OpenQuerySelector<_, T> instance) => instance.Get().Get;

		readonly Selector<_, T[]> _selector;

		public OpenQuerySelector(Selector<_, T[]> selector) => _selector = selector;

		public OpenQuerySelector<_, TOut> Select<TOut>(Func<Array.ValueEnumerableWrapper<T>, TOut[]> select)
			=> Select(new OpenQuery<T, TOut>(select));

		public OpenQuerySelector<_, TOut> Select<TOut>(IOpenQuery<T, TOut> query)
			=> new OpenQuerySelector<_, TOut>(_selector.Select(x => x.AsValueEnumerable()).Select(query));

		public Selector<_, TOut> Reduce<TOut>(Func<Array.ValueEnumerableWrapper<T>, TOut> select)
			=> Reduce(new OpenReduce<T, TOut>(select));

		public Selector<_, TOut> Reduce<TOut>(IOpenReduce<T, TOut> query)
			=> _selector.Select(x => x.AsValueEnumerable()).Select(query);

		public ISelect<_, T[]> Out() => _selector.Get();

		public ISelect<_, Array<T>> Get() => _selector.Select(DragonSpark.Model.Sequences.Result<T>.Default).Get();
	}
}