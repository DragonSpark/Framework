using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class QueryProvider<T> : IQueryProvider
{
	readonly IQueryable<T> _queryable;

	public QueryProvider(IQueryable<T> queryable) => _queryable = queryable;

	public IQueryable CreateQuery(Expression expression) => _queryable;

	public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		=> (IQueryable<TElement>)_queryable;

	public object Execute(Expression expression) => throw new NotSupportedException("Use async execution.");

	public TResult Execute<TResult>(Expression expression) => throw new NotSupportedException("Use async execution.");
}