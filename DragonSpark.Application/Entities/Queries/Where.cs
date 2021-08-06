using System.Linq;

namespace DragonSpark.Application.Entities.Queries
{
	public class Where<TKey, T> : IQuery<TKey, T>
	{
		readonly IQueryable<T>  _source;
		readonly Query<TKey, T> _query;

		protected Where(IQueryable<T> source, Query<TKey, T> query)
		{
			_source = source;
			_query  = query;
		}

		public IQueryable<T> Get(TKey parameter) => _source.Where(_query(parameter));
	}
}