using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime;

public class RuntimeQuery<T> : RuntimeQuery<None, T>, IRuntimeQuery<T>
{
	protected RuntimeQuery(IContexts contexts, IQuery<None, T> query) : base(contexts, query) {}
}

public class RuntimeQuery<TIn, TOut> : IRuntimeQuery<TIn, TOut>
{
	readonly IContexts                                _contexts;
	readonly Func<DbContext, TIn, IQueryable<TOut>> _compiled;

	protected RuntimeQuery(IContexts contexts, IQuery<TIn, TOut> query) : this(contexts, query.Get().Expand().Compile()) {}

	RuntimeQuery(IContexts contexts, Func<DbContext, TIn, IQueryable<TOut>> compiled)
	{
		_contexts   = contexts;
		_compiled = compiled;
	}

	public IQueries<TOut> Get(TIn parameter) => new Queries<TIn, TOut>(_contexts, parameter, _compiled);
}