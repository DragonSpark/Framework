using DragonSpark.Application.Entities.Queries.Model;
using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Scoped
{
	public class WhereSelection<TKey, TEntity, T> : IQuery<TKey, T>
	{
		readonly IQueryable<TEntity>                      _source;
		readonly Express<TKey, TEntity>                     _express;
		readonly Func<IQueryable<TEntity>, IQueryable<T>> _selection;

		public WhereSelection(IQueryable<TEntity> source, Express<TKey, TEntity> express,
		                      Func<IQueryable<TEntity>, IQueryable<T>> selection)
		{
			_source    = source;
			_express     = express;
			_selection = selection;
		}

		public IQueryable<T> Get(TKey parameter) => _selection(_source.Where(_express(parameter)));
	}
}