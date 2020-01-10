using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Query;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;

namespace DragonSpark.Compose.Model
{
	public interface IOpenQuery<in TIn, out TOut> : ISelect<TIn[], TOut[]> {}

	public class OpenWhereQuery<T> : IOpenQuery<T, T>
	{
		readonly Func<T, bool> _condition;

		public OpenWhereQuery(Func<T, bool> condition) => _condition = condition;

		public T[] Get(T[] parameter) => parameter.Where(_condition).ToArray();
	}

	sealed class OpenQuerySelect<TIn, TOut> : IOpenQuery<TIn, TOut>
	{
		readonly Func<TIn, TOut> _select;

		public OpenQuerySelect(Func<TIn, TOut> select) => _select = @select;

		public TOut[] Get(TIn[] parameter) => parameter.Select(_select).ToArray();
	}

	sealed class OpenQuery<TIn, TOut> : DragonSpark.Model.Selection.Select<TIn[], TOut[]>, IOpenQuery<TIn, TOut>
	{
		public OpenQuery(Func<TIn[], TOut[]> select) : base(select) {}
	}

	public interface IOpenReduce<in TIn, out TOut> : ISelect<TIn[], TOut> {}

	sealed class OpenReduce<TIn, TOut> : DragonSpark.Model.Selection.Select<TIn[], TOut>, IOpenReduce<TIn, TOut>
	{
		public OpenReduce(Func<TIn[], TOut> select) : base(select) {}
	}

	public interface ISequenceQuery<in TIn, out TOut> : ISelect<IEnumerable<TIn>, TOut[]> {}

	sealed class SequenceQuery<TIn, TOut> : DragonSpark.Model.Selection.Select<IEnumerable<TIn>, TOut[]>,
	                                        ISequenceQuery<TIn, TOut>
	{
		public SequenceQuery(Func<IEnumerable<TIn>, TOut[]> select) : base(select) {}
	}

	public interface ISequenceReduce<TIn, out TOut> : ISelect<IEnumerable<TIn>, TOut> {}

	sealed class SequenceReduce<TIn, TOut> : DragonSpark.Model.Selection.Select<IEnumerable<TIn>, TOut>,
	                                         ISequenceReduce<TIn, TOut>
	{
		public SequenceReduce(Func<IEnumerable<TIn>, TOut> select) : base(select) {}
	}

	public sealed class FirstAssigned<T> : FirstWhere<T>
	{
		public static FirstAssigned<T> Default { get; } = new FirstAssigned<T>();

		FirstAssigned() : base(Is.Assigned<T>()) {}

		public FirstAssigned(ISelect<T, bool> @where) : base(Is.Assigned<T>().And(where)) {}
	}

	public class FirstWhere<T> : IOpenReduce<T, T>
	{
		readonly Func<T>       _default;
		readonly Func<T, bool> _where;

		public FirstWhere(ICondition<T> where) : this(where.Get) {}

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

	public class One<T> : IOpenReduce<T, T>
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

	// TODO: name
	public sealed class SequenceQuerySelector<_, T> : IResult<ISelect<_, Array<T>>>
	{
		public static implicit operator Func<_, Array<T>>(SequenceQuerySelector<_, T> instance) => instance.Get().Get;

		readonly Selector<_, IEnumerable<T>> _selector;

		public SequenceQuerySelector(Selector<_, IEnumerable<T>> selector) => _selector = selector;

		public OpenQuerySelector<_, TOut> Select<TOut>(Func<IEnumerable<T>, TOut[]> select)
			=> Select(new SequenceQuery<T, TOut>(select));

		public OpenQuerySelector<_, TOut> Select<TOut>(ISequenceQuery<T, TOut> query)
			=> new OpenQuerySelector<_, TOut>(_selector.Select(query));

		public Selector<_, TOut> Reduce<TOut>(Func<IEnumerable<T>, TOut> select)
			=> Reduce(new SequenceReduce<T, TOut>(select));

		public Selector<_, TOut> Reduce<TOut>(ISequenceReduce<T, TOut> query)
			=> _selector.Select(query);

		public ISelect<_, T[]> Out() => _selector.Get().Open();

		public ISelect<_, Array<T>> Get() => _selector.Select(DragonSpark.Model.Sequences.Result<T>.Default).Get();
	}

	// TODO: name
	public sealed class OpenQuerySelector<_, T> : IResult<ISelect<_, Array<T>>>
	{
		public static implicit operator Func<_, Array<T>>(OpenQuerySelector<_, T> instance) => instance.Get().Get;

		readonly Selector<_, T[]> _selector;

		public OpenQuerySelector(Selector<_, T[]> selector) => _selector = selector;

		public OpenQuerySelector<_, TOut> Query<TOut>(Func<T[], TOut[]> select)
			=> Query(new OpenQuery<T, TOut>(select));

		public OpenQuerySelector<_, TOut> Query<TOut>(IOpenQuery<T, TOut> query)
			=> new OpenQuerySelector<_, TOut>(_selector.Select(query));

		public OpenQuerySelector<_, TOut> Select<TOut>(Func<T, TOut> select)
			=> Query(new OpenQuerySelect<T, TOut>(select));

		public OpenQuerySelector<_, T> Where(Func<T, bool> condition) => Query(new OpenWhereQuery<T>(condition));

		public Selector<_, TOut> Reduce<TOut>(Func<T[], TOut> select) => Reduce(new OpenReduce<T, TOut>(select));

		public Selector<_, TOut> Reduce<TOut>(IOpenReduce<T, TOut> query) => _selector.Select(query);

		public Selector<_, T> FirstAssigned(ISelect<T, bool> where) => Reduce(new FirstAssigned<T>(where));

		public Selector<_, T> FirstAssigned() => Reduce(FirstAssigned<T>.Default);

		public Selector<_, T> Only(Func<T, bool> where) => Reduce(new Only<T>(where));

		public Selector<_, T> Only() => Reduce(Only<T>.Default);

		public ISelect<_, T[]> Out() => _selector.Get();

		public ISelect<_, Array<T>> Get() => _selector.Select(DragonSpark.Model.Sequences.Result<T>.Default).Get();
	}
}