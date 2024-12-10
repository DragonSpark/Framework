using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime;

sealed class Queries<TIn, TOut>(IScopes scopes, TIn parameter, Func<DbContext, TIn, IQueryable<TOut>> compiled)
	: IQueries<TOut>
{
	[MustDisposeResource]
	public ValueTask<Query<TOut>> Get()
	{
		var (context, disposable) = scopes.Get();
		var query = compiled(context, parameter);
		return new(new Query<TOut>(query, disposable));
	}
}
