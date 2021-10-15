using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime;

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

	public async ValueTask<Query<TOut>> Get()
	{
		var (subject, boundary) = _scopes.Get();
		var query = _compiled(subject, _parameter);
		var start = await boundary.Await();
		return new(query, start);
	}
}