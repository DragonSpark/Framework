﻿using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Query;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Compose.Model
{
	// TODO: simplify

	sealed class GroupMapAdapter<T, TKey> : ISelect<Array<T>, IArrayMap<TKey, T>>
	{
		readonly IReduce<T, IArrayMap<TKey, T>> _reduce;

		public GroupMapAdapter(IReduce<T, IArrayMap<TKey, T>> reduce) => _reduce = reduce;

		public IArrayMap<TKey, T> Get(Array<T> parameter) => _reduce.Get(parameter.Open());
	}

	/*public interface IMaterializedQuery<in TIn, out TOut> : ISelect<TIn[], IEnumerable<TOut>>, IQuery<TIn, TOut> {}*/

	public class WhereMaterialization<T> : IMaterialization<T, T>
	{
		readonly Func<T, bool> _condition;

		public WhereMaterialization(Func<T, bool> condition) => _condition = condition;

		public T[] Get(T[] parameter) => parameter.Where(_condition).ToArray();

		public T[] Get(IEnumerable<T> parameter) => parameter.AsValueEnumerable().Where(_condition).ToArray();
	}

	sealed class MaterializedSelect<TIn, TOut> : IMaterialization<TIn, TOut>
	{
		readonly Func<TIn, TOut> _select;

		public MaterializedSelect(Func<TIn, TOut> select) => _select = select;

		public TOut[] Get(TIn[] parameter) => parameter.Select(_select).ToArray();

		public TOut[] Get(IEnumerable<TIn> parameter) => parameter.AsValueEnumerable().Select(_select).ToArray();
	}

	sealed class Materialization<TIn, TOut> : DragonSpark.Model.Selection.Select<TIn[], TOut[]>, IMaterialization<TIn, TOut>
	{
		readonly Func<TIn[], TOut[]> _select;

		public Materialization(Func<TIn[], TOut[]> select) : base(select) => _select = select;

		public TOut[] Get(IEnumerable<TIn> parameter) => parameter.Open().To(_select);
	}

	public interface IReduce<in TIn, out TOut> : ISelect<IEnumerable<TIn>, TOut> {}

	public interface IReduction<in TIn, out TOut> : ISelect<TIn[], TOut>, IReduce<TIn, TOut> {}

	public interface IMaterialize<in TIn, out TOut> : IReduce<TIn, TOut[]> {}

	public interface IMaterialization<in TIn, out TOut> : IReduction<TIn, TOut[]>, IMaterialize<TIn, TOut> {}

	sealed class Reduction<TIn, TOut> : IReduction<TIn, TOut>
	{
		readonly Func<TIn[], TOut> _select;

		public Reduction(Func<TIn[], TOut> select) => _select = select;

		public TOut Get(IEnumerable<TIn> parameter) => _select(parameter.ToArray());

		public TOut Get(TIn[] parameter) => _select(parameter);
	}

	public interface ISequenceQuery<in TIn, out TOut> : ISelect<IEnumerable<TIn>, TOut[]> {}

	sealed class SequenceQuery<TIn, TOut> : DragonSpark.Model.Selection.Select<IEnumerable<TIn>, TOut[]>,
	                                        ISequenceQuery<TIn, TOut>
	{
		public SequenceQuery(Func<IEnumerable<TIn>, TOut[]> select) : base(select) {}
	}

	public interface ISequenceReduce<in TIn, out TOut> : ISelect<IEnumerable<TIn>, TOut> {}

	sealed class SequenceReduce<TIn, TOut> : DragonSpark.Model.Selection.Select<IEnumerable<TIn>, TOut>,
	                                         ISequenceReduce<TIn, TOut>
	{
		public SequenceReduce(Func<IEnumerable<TIn>, TOut> select) : base(select) {}
	}

	public sealed class FirstAssigned<T> : FirstWhere<T>
	{
		public static FirstAssigned<T> Default { get; } = new FirstAssigned<T>();

		FirstAssigned() : base(Is.Assigned<T>()) {}

		public FirstAssigned(ISelect<T, bool> where) : base(Is.Assigned<T>().And(where)) {}
	}

	public class FirstWhere<T> : ISelect<T[], T>
	{
		readonly Func<T>       _default;
		readonly Func<T, bool> _where;

		public FirstWhere(Func<T, bool> where) : this(where, () => default) {}

		public FirstWhere(Func<T, bool> where, Func<T> @default)
		{
			_where   = where;
			_default = @default;
		}

		public T Get(T[] parameter)
		{
			var length = parameter.Length;
			for (var i = 0u; i < length; i++)
			{
				var item = parameter[i];
				if (_where(item))
				{
					return item;
				}
			}

			return _default();
		}
	}

	public class One<T> : ISelect<T[], T>
	{
		readonly Func<T>                          _default;
		readonly Func<ArrayView<T>, ArrayView<T>> _where;

		public One(Func<T, bool> where) : this(where, A.Default<T>) {}

		public One(Func<T, bool> where, Func<T> @default)
			: this(new Where<T>(where, Selection.Default, 2).Get, @default) {}

		public One(Func<ArrayView<T>, ArrayView<T>> where, Func<T> @default)
		{
			_where   = where;
			_default = @default;
		}

		public T Get(T[] parameter)
		{
			var view   = _where(new ArrayView<T>(parameter, 0, (uint)parameter.Length));
			var result = view.Length == 1 ? view.Array[0] : _default();
			return result;
		}
	}

	public sealed class Only<T> : One<T>
	{
		public static Only<T> Default { get; } = new Only<T>();

		Only() : this(Always<T>.Default.Get) {}

		public Only(Func<T, bool> where) : base(where) {}
	}

	public sealed class SequencedQuery<_, T> : IResult<ISelect<_, Array<T>>>
	{
		public static implicit operator Func<_, Array<T>>(SequencedQuery<_, T> instance) => instance.Get().Get;

		readonly Selector<_, IEnumerable<T>> _selector;

		public SequencedQuery(Selector<_, IEnumerable<T>> selector) => _selector = selector;

		public MaterializedQuery<_, TOut> Select<TOut>(Func<IEnumerable<T>, TOut[]> select)
			=> Select(new SequenceQuery<T, TOut>(select));

		public MaterializedQuery<_, TOut> Select<TOut>(ISequenceQuery<T, TOut> query)
			=> new MaterializedQuery<_, TOut>(_selector.Select(query));

		public Selector<_, TOut> Reduce<TOut>(Func<IEnumerable<T>, TOut> select)
			=> Reduce(new SequenceReduce<T, TOut>(select));

		public Selector<_, TOut> Reduce<TOut>(ISequenceReduce<T, TOut> query)
			=> _selector.Select(query);

		public ISelect<_, T[]> Out() => _selector.Get().Open();

		public ISelect<_, Array<T>> Get() => _selector.Select(DragonSpark.Model.Sequences.Result<T>.Default).Get();
	}

	public sealed class MaterializedQuery<_, T> : IResult<ISelect<_, Array<T>>>
	{
		public static implicit operator Func<_, Array<T>>(MaterializedQuery<_, T> instance) => instance.Get().Get;

		readonly Selector<_, T[]> _selector;

		public MaterializedQuery(Selector<_, T[]> selector) => _selector = selector;

		public MaterializedQuery<_, TOut> Query<TOut>(Func<T[], TOut[]> select)
			=> Query<TOut>(new Materialization<T, TOut>(select));

		public MaterializedQuery<_, TOut> Query<TOut>(ISelect<T[], TOut[]> query)
			=> new MaterializedQuery<_, TOut>(_selector.Select(query));

		public MaterializedQuery<_, TOut> Select<TOut>(Func<T, TOut> select)
			=> Query<TOut>(new MaterializedSelect<T, TOut>(select));

		public MaterializedQuery<_, T> Where(Func<T, bool> condition) => Query<T>(new WhereMaterialization<T>(condition));

		public MaterializedQuery<_, T> Distinct() => Query(x => x.AsValueEnumerable().Distinct().ToArray());

		public Selector<_, TOut> Reduce<TOut>(Func<T[], TOut> select) => Reduce<TOut>(new Reduction<T, TOut>(select));

		public Selector<_, TOut> Reduce<TOut>(ISelect<T[], TOut> query) => _selector.Select(query);

		public Selector<_, T> FirstAssigned(ISelect<T, bool> where) => Reduce(new FirstAssigned<T>(where));

		public Selector<_, T> FirstAssigned() => Reduce(FirstAssigned<T>.Default);

		public Selector<_, T> Only(Func<T, bool> where) => Reduce(new Only<T>(where));

		public Selector<_, T> Only() => Reduce(Only<T>.Default);

		public ISelect<_, T[]> Out() => _selector.Get();

		public ISelect<_, Array<T>> Get() => _selector.Select(DragonSpark.Model.Sequences.Result<T>.Default).Get();

		public Selector<_, T> FirstOrDefault() => Reduce(x => x.AsValueEnumerable().FirstOrDefault());
	}
}