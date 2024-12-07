using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public class EvaluateToPage<TIn, T> : ISelecting<PageRequest<TIn>, PageResponse<T>>
{
	readonly IScopes                                         _scopes;
	readonly Expression<Func<DbContext, TIn, IQueryable<T>>> _expression;
	readonly IFilter<T>                                      _filter;

	protected EvaluateToPage(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(scopes, expression, Filter<T>.Default) {}

	protected EvaluateToPage(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression,
	                         IFilter<T> filter)
	{
		_scopes     = scopes;
		_expression = expression;
		_filter     = filter;
	}

	public async ValueTask<PageResponse<T>> Get(PageRequest<TIn> parameter)
	{
		using var scope    = _scopes.Get();
		var       query    = _expression.Invoke(scope.Owner, parameter.Subject).AsExpandable();
		var       filtered = parameter.Filter is not null ? _filter.Get(new(query, parameter.Filter)) : query;
		var       all      = parameter.OrderBy is not null ? filtered.OrderBy(parameter.OrderBy) : filtered;
		var       count    = parameter.Count ? (uint)await all.CountAsync().Await() : (uint?)null;
		var       paged    = all.Skip((int)(parameter.Skip ?? 0)).Take(parameter.Top ?? 10);
		var       page     = await paged.ToArrayAsync().Await();
		return new(page, count);
	}
}