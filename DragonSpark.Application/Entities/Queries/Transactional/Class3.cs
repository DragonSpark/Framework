namespace DragonSpark.Application.Entities.Queries.Transactional
{
	class Class3 {}

	/*public interface IQuery<in TIn, TOut> : ISelect<TIn, Query<TOut>> where TOut : class {}

	public class Query<TIn, T> : Query<TIn, T, T> where T : class
	{
		public Query(IQuery<T> query, ISelector<TIn, T, T> select) : base(query, select) {}

		public Query(IQuery<TIn, T> query, ISelector<TIn, T, T> select) : base(query, select) {}
	}

	public class Query<TIn, T, TOut> : IQuery<TIn, TOut> where TOut : class where T : class
	{
		readonly IQuery<TIn, T>          _query;
		readonly ISelector<TIn, T, TOut> _select;

		public Query(IQuery<T> query, ISelector<TIn, T, TOut> select) : this(new Accept<TIn, T>(query), select) {}

		public Query(IQuery<TIn, T> query, ISelector<TIn, T, TOut> select)
		{
			_query  = query;
			_select = select;
		}

		public Query<TOut> Get(TIn parameter)
		{
			var session = _query.Get(parameter);
			var query   = _select.Get(session.In(parameter));
			var result  = session.Select(query);
			return result;
		}
	}

	public sealed class Accept<TIn, T> : DelegatedResult<TIn, Query<T>>, IQuery<TIn, T> where T : class
	{
		public Accept(IQuery<T> instance) : base(instance.Get) {}
	}

	public class Where<TIn, TOut> : Query<TIn, TOut> where TOut : class
	{
		protected Where(IQuery<TOut> query, Express<TIn, TOut> select)
			: base(query, new WhereSelector<TIn, TOut>(select)) {}

		protected Where(IQuery<TIn, TOut> query, Express<TIn, TOut> select)
			: base(query, new WhereSelector<TIn, TOut>(select)) {}
	}

	public class WhereMany<TIn, TFrom, TTo> : Query<TIn, TFrom, TTo> where TFrom : class where TTo : class
	{
		protected WhereMany(IQuery<TFrom> query, Express<TIn, TFrom> where,
		                    Expression<Func<TFrom, IEnumerable<TTo>>> select)
			: base(query, new WhereManySelector<TIn, TFrom, TTo>(where, select)) {}

		protected WhereMany(IQuery<TIn, TFrom> query, Express<TIn, TFrom> where,
		                    Expression<Func<TFrom, IEnumerable<TTo>>> select)
			: base(query, new WhereManySelector<TIn, TFrom, TTo>(where, select)) {}
	}

	public class WhereSelect<TIn, TFrom, TTo> : Query<TIn, TFrom, TTo> where TFrom : class where TTo : class
	{
		protected WhereSelect(IQuery<TFrom> query, Express<TIn, TFrom> where, Expression<Func<TFrom, TTo>> select)
			: base(query, new WhereSelectSelector<TIn, TFrom, TTo>(where, select)) {}

		protected WhereSelect(IQuery<TIn, TFrom> query, Express<TIn, TFrom> where, Expression<Func<TFrom, TTo>> select)
			: base(query, new WhereSelectSelector<TIn, TFrom, TTo>(where, select)) {}
	}

	public class WhereSelection<TIn, TFrom, TTo> : Query<TIn, TFrom, TTo> where TFrom : class where TTo : class
	{
		protected WhereSelection(IQuery<TFrom> query, Express<TIn, TFrom> where,
		                         Func<IQueryable<TFrom>, IQueryable<TTo>> selection)
			: base(query, new WhereSelectionSelector<TIn, TFrom, TTo>(where, selection)) {}

		protected WhereSelection(IQuery<TIn, TFrom> query, Express<TIn, TFrom> where,
		                         Func<IQueryable<TFrom>, IQueryable<TTo>> selection)
			: base(query, new WhereSelectionSelector<TIn, TFrom, TTo>(where, selection)) {}
	}

	public class ParameterAwareSelection<TIn, TFrom, TTo> : Query<TIn, TFrom, TTo> where TTo : class where TFrom : class
	{
		protected ParameterAwareSelection(IQuery<TFrom> query, Func<TIn, IQueryable<TFrom>, IQueryable<TTo>> selection)
			: base(query, new ParameterAwareSelectionSelector<TIn, TFrom, TTo>(selection)) {}

		protected ParameterAwareSelection(IQuery<TIn, TFrom> query,
		                                  Func<TIn, IQueryable<TFrom>, IQueryable<TTo>> selection)
			: base(query, new ParameterAwareSelectionSelector<TIn, TFrom, TTo>(selection)) {}
	}

	public class ParameterAwareWhereSelection<TIn, TFrom, TTo> : Query<TIn, TFrom, TTo>
		where TTo : class where TFrom : class
	{
		protected ParameterAwareWhereSelection(IQuery<TFrom> query, Express<TIn, TFrom> where,
		                                       Func<TIn, IQueryable<TFrom>, IQueryable<TTo>> selection)
			: base(query, new ParameterAwareWhereSelectionSelector<TIn, TFrom, TTo>(where, selection)) {}

		protected ParameterAwareWhereSelection(IQuery<TIn, TFrom> query, Express<TIn, TFrom> where,
		                                       Func<TIn, IQueryable<TFrom>, IQueryable<TTo>> selection)
			: base(query, new ParameterAwareWhereSelectionSelector<TIn, TFrom, TTo>(where, selection)) {}
	}*/
}