using System.Linq;

namespace DragonSpark.Application.Entities.Queries
{
	public class ParameterAwareWhereSelect<TKey, TEntity, T> : IQuery<TKey, T>
	{
		readonly IQuery<TKey, TEntity>   _select;
		readonly Query<TKey, TEntity>    _where;
		readonly Query<TKey, TEntity, T> _selection;

		protected ParameterAwareWhereSelect(IQueryable<TEntity> queryable, Query<TKey, TEntity> where,
		                                    Query<TKey, TEntity, T> selection)
			: this(new Accept<TKey, TEntity>(queryable), where, selection) {}

		protected ParameterAwareWhereSelect(IQuery<TKey, TEntity> select, Query<TKey, TEntity> where,
		                                    Query<TKey, TEntity, T> selection)
		{
			_select    = select;
			_where     = where;
			_selection = selection;
		}

		public IQueryable<T> Get(TKey parameter)
			=> _select.Get(parameter).Where(_where(parameter)).Select(_selection(parameter));
	}
}