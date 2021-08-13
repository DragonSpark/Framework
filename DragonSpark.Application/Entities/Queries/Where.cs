using System.Linq;

namespace DragonSpark.Application.Entities.Queries
{
	public class Where<TKey, T> : IQuery<TKey, T>
	{
		readonly IQuery<TKey, T> _query;
		readonly Query<TKey, T>  _select;

		public Where(IQueryable<T> query, Query<TKey, T> select)
			: this(new Accept<TKey, T>(query), @select) {}

		public Where(IQuery<TKey, T> query, Query<TKey, T> select)
		{
			_query  = query;
			_select = select;
		}

		public IQueryable<T> Get(TKey parameter) => _query.Get(parameter).Where(_select(parameter));
	}
}