using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Scoped
{
	public class ParameterAwareWhere<TKey, T> : IQuery<TKey, T>
	{
		readonly IQuery<TKey, T> _select;
		readonly Express<TKey, T>  _where;

		protected ParameterAwareWhere(IQueryable<T> queryable, Express<TKey, T> where)
			: this(new Accept<TKey, T>(queryable), where) {}

		protected ParameterAwareWhere(IQuery<TKey, T> select, Express<TKey, T> where)
		{
			_select = select;
			_where  = where;
		}

		public IQueryable<T> Get(TKey parameter) => _select.Get(parameter).Where(_where(parameter));
	}
}