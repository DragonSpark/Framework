using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries
{
	public class WhereSelect<TKey, TEntity, T> : IQuery<TKey, T>
	{
		readonly IQueryable<TEntity>          _source;
		readonly Query<TKey, TEntity>         _query;
		readonly Expression<Func<TEntity, T>> _select;

		public WhereSelect(IQueryable<TEntity> source, Query<TKey, TEntity> query,
		                   Expression<Func<TEntity, T>> select)
		{
			_source = source;
			_query  = query;
			_select = select;
		}

		public IQueryable<T> Get(TKey parameter) => _source.Where(_query(parameter)).Select(_select);
	}
}