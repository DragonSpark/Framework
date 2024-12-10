using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime;

sealed class Queries<TIn, TOut> : IQueries<TOut>
{
	readonly IScopes                                _scopes;
	readonly TIn                                    _parameter;
	readonly Func<DbContext, TIn, IQueryable<TOut>> _compiled;

	public Queries(IScopes scopes, TIn parameter, Func<DbContext, TIn, IQueryable<TOut>> compiled)
	{
		_scopes    = scopes;
		_parameter = parameter;
		_compiled  = compiled;
	}

	[MustDisposeResource]
	public ValueTask<Query<TOut>> Get()
	{
		var (context, disposable) = _scopes.Get();
		var query   = _compiled(context, _parameter);
		return new(new Query<TOut>(query, disposable));
	}
}
