using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime;

sealed class Queries<TIn, TOut> : IQueries<TOut>
{
	readonly IContexts                                _contexts;
	readonly TIn                                    _parameter;
	readonly Func<DbContext, TIn, IQueryable<TOut>> _compiled;

	public Queries(IContexts contexts, TIn parameter, Func<DbContext, TIn, IQueryable<TOut>> compiled)
	{
		_contexts    = contexts;
		_parameter = parameter;
		_compiled  = compiled;
	}

	public Query<TOut> Get()
	{
		var (context, disposable) = _contexts.Get();
		var query   = _compiled(context, _parameter);
		return new(query, disposable);
	}
}