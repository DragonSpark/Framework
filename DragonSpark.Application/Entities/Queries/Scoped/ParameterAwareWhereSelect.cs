using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Scoped
{
	public class ParameterAwareWhereSelect<TKey, TEntity, T> : IQuery<TKey, T>
	{
		readonly IQuery<TKey, TEntity>   _select;
		readonly Express<TKey, TEntity>    _where;
		readonly Express<TKey, TEntity, T> _selection;

		protected ParameterAwareWhereSelect(IQueryable<TEntity> queryable, Express<TKey, TEntity> where,
		                                    Express<TKey, TEntity, T> selection)
			: this(new Accept<TKey, TEntity>(queryable), where, selection) {}

		protected ParameterAwareWhereSelect(IQuery<TKey, TEntity> select, Express<TKey, TEntity> where,
		                                    Express<TKey, TEntity, T> selection)
		{
			_select    = select;
			_where     = where;
			_selection = selection;
		}

		public IQueryable<T> Get(TKey parameter)
			=> _select.Get(parameter).Where(_where(parameter)).Select(_selection(parameter));
	}
}