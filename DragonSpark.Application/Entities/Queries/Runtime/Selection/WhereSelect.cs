using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Runtime.Selection
{
	public class WhereSelect<TKey, TEntity, T> : IQuery<TKey, T>
	{
		readonly IQueryable<TEntity>          _source;
		readonly Express<TKey, TEntity>         _express;
		readonly Expression<Func<TEntity, T>> _select;

		public WhereSelect(IQueryable<TEntity> source, Express<TKey, TEntity> express,
		                   Expression<Func<TEntity, T>> select)
		{
			_source = source;
			_express  = express;
			_select = select;
		}

		public IQueryable<T> Get(TKey parameter) => _source.Where(_express(parameter)).Select(_select);
	}
}