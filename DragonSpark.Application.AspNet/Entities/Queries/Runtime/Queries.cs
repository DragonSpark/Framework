using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime;

sealed class Queries<TIn, TOut>(IScopes scopes, TIn parameter, Func<DbContext, TIn, IQueryable<TOut>> compiled)
	: IQueries<TOut>
{
	readonly TIn _parameter = parameter;

	[MustDisposeResource]
	public ValueTask<Query<TOut>> Get(CancellationToken parameter)
	{
		var (context, disposable) = scopes.Get();
		var query = compiled(context, _parameter);
		return new(new Query<TOut>(query, disposable));
	}
}
