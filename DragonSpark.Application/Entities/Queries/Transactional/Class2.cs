using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Transactional
{
	class Class2 {}

	public interface IMaps : IAssignable<Type, IMap> {}

	public interface IMap : IAsyncDisposable
	{
		IQueryable<T> Query<T>() where T : class;
	}

	public interface IQuery<out T> : ISelect<IMaps, IQueryable<T>> {}

	public class DbQueryBase<TContext, T> : QueryBase<T> where TContext : DbContext where T : class
	{
		protected DbQueryBase(DbQuery<TContext, T> previous) : base(previous) {}

		protected DbQueryBase(DbQuery<TContext, T> previous, Func<IQueryable<T>, IQueryable<T>> query)
			: base(previous, query) {}

		protected DbQueryBase(IQuery<T> previous) : base(previous) {}
	}

	public class QueryBase<T> : IQuery<T>
	{
		readonly IQuery<T>                          _previous;
		readonly Func<IQueryable<T>, IQueryable<T>> _query;

		protected QueryBase(IQuery<T> previous) : this(previous, x => x) {}

		protected QueryBase(IQuery<T> previous, Func<IQueryable<T>, IQueryable<T>> query)
		{
			_previous = previous;
			_query    = query;
		}

		public IQueryable<T> Get(IMaps parameter) => _query(_previous.Get(parameter));
	}

	public class DbQuery<TContext, T> : MapAwareQuery<T> where TContext : DbContext where T : class
	{
		public DbQuery(DbQueryMaps<TContext> maps) : base(maps) {}
	}

	public class MapAwareQuery<T> : IQuery<T> where T : class
	{
		readonly IQuery<T>  _previous;
		readonly IQueryMaps _maps;
		readonly Type       _key;

		protected MapAwareQuery(IQueryMaps maps) : this(maps, A.Type<T>()) {}

		protected MapAwareQuery(IQueryMaps maps, Type key) : this(new Root<T>(key), maps, key) {}

		protected MapAwareQuery(IQuery<T> previous, IQueryMaps maps, Type key)
		{
			_previous = previous;
			_maps     = maps;
			_key      = key;
		}

		public IQueryable<T> Get(IMaps parameter)
		{
			if (!parameter.IsSatisfiedBy(_key))
			{
				parameter.Assign(_key, _maps.Get());
			}

			return _previous.Get(parameter);
		}
	}

	sealed class Root<T> : IQuery<T> where T : class
	{
		readonly Type _context;

		public Root(Type context) => _context = context;

		public IQueryable<T> Get(IMaps parameter) => parameter.Get(_context).Query<T>();
	}

	/*public interface IQuery<TIn, out TOut> : ISelect<In<TIn>, IQueryable<TOut>> {}

	sealed class Start<T> : IQuery<T> where T : class
	{
		public static Start<T> Default { get; } = new Start<T>();

		Start() {}

		public IQueryable<T> Get(In<T> parameter) => parameter.Context.Set<T>();
	}*/

	/*
	public readonly struct In<TIn, T>
	{
		readonly DbContext _source;

		public In(DbContext source, IQueryable<T> query, TIn parameter)
		{
			_source   = source;
			Query     = query;
			Parameter = parameter;
		}

		public IQueryable<T> Query { get; }

		public TIn Parameter { get; }

		public void Deconstruct(out DbContext source, out IQueryable<T> query, out TIn parameter)
		{
			source    = _source;
			query     = Query;
			parameter = Parameter;
		}
	}

	public interface ISelector<TIn, TOut> : ISelector<TIn, TOut, TOut> {}

	public interface ISelector<TIn, T, out TOut> : ISelect<In<TIn, T>, IQueryable<TOut>> {}

	public class WhereSelector<TIn, T> : ISelector<TIn, T>
	{
		readonly Express<TIn, T> _select;

		public WhereSelector(Expression<Func<T, bool>> where) : this(where.Start().Accept<TIn>().Get().Get) {}

		public WhereSelector(Express<TIn, T> select) => _select = select;

		public IQueryable<T> Get(In<TIn, T> parameter) => parameter.Query.Where(_select(parameter.Parameter));
	}

	public class SelectSelector<TIn, TFrom, TTo> : ISelector<TIn, TFrom, TTo>
	{
		readonly Express<TIn, TFrom, TTo> _select;

		public SelectSelector(Expression<Func<TFrom, TTo>> select) : this(select.Start().Accept<TIn>().Get().Get) {}

		public SelectSelector(Express<TIn, TFrom, TTo> select) => _select = @select;

		public IQueryable<TTo> Get(In<TIn, TFrom> parameter) => parameter.Query.Select(_select(parameter.Parameter));
	}

	public class SelectManySelector<TIn, TFrom, TTo> : ISelector<TIn, TFrom, TTo>
	{
		readonly Express<TIn, TFrom, IEnumerable<TTo>> _select;

		public SelectManySelector(Expression<Func<TFrom, IEnumerable<TTo>>> select)
			: this(select.Start().Accept<TIn>().Get().Get) {}

		public SelectManySelector(Express<TIn, TFrom, IEnumerable<TTo>> select) => _select = @select;

		public IQueryable<TTo> Get(In<TIn, TFrom> parameter)
			=> parameter.Query.SelectMany(_select(parameter.Parameter));
	}

	public class SelectionSelector<TIn, TFrom, TTo> : ISelector<TIn, TFrom, TTo>
	{
		readonly Func<TIn, Func<IQueryable<TFrom>, IQueryable<TTo>>> _selection;

		public SelectionSelector(Func<IQueryable<TFrom>, IQueryable<TTo>> select)
			: this(Start.A.Result(select).Accept<TIn>()) {}

		public SelectionSelector(Func<TIn, Func<IQueryable<TFrom>, IQueryable<TTo>>> selection)
			=> _selection = selection;

		public IQueryable<TTo> Get(In<TIn, TFrom> parameter) => _selection(parameter.Parameter)(parameter.Query);
	}

	public class WhereManySelector<TIn, TFrom, TTo> : ISelector<TIn, TFrom, TTo>
	{
		readonly Express<TIn, TFrom>                       _where;
		readonly Expression<Func<TFrom, IEnumerable<TTo>>> _select;

		public WhereManySelector(Express<TIn, TFrom> where, Expression<Func<TFrom, IEnumerable<TTo>>> select)
		{
			_where  = @where;
			_select = @select;
		}

		public IQueryable<TTo> Get(In<TIn, TFrom> parameter)
			=> parameter.Query.Where(_where(parameter.Parameter)).SelectMany(_select);
	}

	public class WhereSelectSelector<TIn, TFrom, TTo> : ISelector<TIn, TFrom, TTo>
	{
		readonly Express<TIn, TFrom>          _where;
		readonly Expression<Func<TFrom, TTo>> _select;

		public WhereSelectSelector(Express<TIn, TFrom> where, Expression<Func<TFrom, TTo>> select)
		{
			_where  = @where;
			_select = @select;
		}

		public IQueryable<TTo> Get(In<TIn, TFrom> parameter)
			=> parameter.Query.Where(_where(parameter.Parameter)).Select(_select);
	}

	public class WhereSelectionSelector<TIn, TFrom, TTo> : ISelector<TIn, TFrom, TTo>
	{
		readonly Express<TIn, TFrom>                      _where;
		readonly Func<IQueryable<TFrom>, IQueryable<TTo>> _selection;

		public WhereSelectionSelector(Express<TIn, TFrom> where, Func<IQueryable<TFrom>, IQueryable<TTo>> selection)
		{
			_where     = where;
			_selection = selection;
		}

		public IQueryable<TTo> Get(In<TIn, TFrom> parameter)
			=> _selection(parameter.Query.Where(_where(parameter.Parameter)));
	}

	public class ParameterAwareSelectionSelector<TIn, TFrom, TTo> : ISelector<TIn, TFrom, TTo>
	{
		readonly Func<TIn, IQueryable<TFrom>, IQueryable<TTo>> _selection;

		public ParameterAwareSelectionSelector(Func<TIn, IQueryable<TFrom>, IQueryable<TTo>> selection)
			=> _selection = selection;

		public IQueryable<TTo> Get(In<TIn, TFrom> parameter) => _selection(parameter.Parameter, parameter.Query);
	}

	public class ParameterAwareWhereSelectionSelector<TIn, TFrom, TTo> : ISelector<TIn, TFrom, TTo>
	{
		readonly Express<TIn, TFrom>                           _where;
		readonly Func<TIn, IQueryable<TFrom>, IQueryable<TTo>> _selection;

		public ParameterAwareWhereSelectionSelector(Express<TIn, TFrom> where,
		                                            Func<TIn, IQueryable<TFrom>, IQueryable<TTo>> selection)
		{
			_where     = where;
			_selection = selection;
		}

		public IQueryable<TTo> Get(In<TIn, TFrom> parameter)
		{
			var (_, queryable, @in) = parameter;
			var result = _selection(@in, queryable.Where(_where(@in)));
			return result;
		}
	}

	public class ExceptSelector<TIn, T> : ISelector<TIn, T> where T : class
	{
		readonly ISelector<TIn, T> _other;

		public ExceptSelector(ISelector<TIn, T> other) => _other = other;

		public IQueryable<T> Get(In<TIn, T> parameter)
		{
			var other  = _other.Get(parameter);
			var result = parameter.Query.Except(other);
			return result;
		}
	}*/
}