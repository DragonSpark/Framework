using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Entities.Queries
{
	class Class3 {}

	public interface IQuery<in TIn, TOut> : ISelect<TIn, Query<TOut>> where TOut : class {}

	public class Query<TIn, T> : Query<T, TIn, T> where T : class
	{
		protected Query(IQuery<T> query, ISelector<T, TIn, T> select) : base(query, select) {}

		protected Query(IQuery<TIn, T> query, ISelector<T, TIn, T> select) : base(query, select) {}
	}

	public class Query<T, TIn, TOut> : IQuery<TIn, TOut> where TOut : class where T : class
	{
		readonly IQuery<TIn, T>          _query;
		readonly ISelector<T, TIn, TOut> _select;

		protected Query(IQuery<T> query, ISelector<T, TIn, TOut> select) : this(new Accept<TIn, T>(query), select) {}

		protected Query(IQuery<TIn, T> query, ISelector<T, TIn, TOut> select)
		{
			_query  = query;
			_select = select;
		}

		public Query<TOut> Get(TIn parameter)
		{
			var session = _query.Get(parameter);
			var query   = _select.Get(new(session.Subject, parameter));
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
}