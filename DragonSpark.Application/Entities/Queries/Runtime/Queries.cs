using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime
{
	sealed class Queries<TIn, TOut> : IQueries<TOut>
	{
		readonly IInvocations                           _invocations;
		readonly TIn                                    _parameter;
		readonly Func<DbContext, TIn, IQueryable<TOut>> _compiled;

		public Queries(IInvocations invocations, TIn parameter, Func<DbContext, TIn, IQueryable<TOut>> compiled)
		{
			_invocations = invocations;
			_parameter   = parameter;
			_compiled    = compiled;
		}

		public async ValueTask<Query<TOut>> Get()
		{
			var (subject, boundary) = _invocations.Get();
			var query = _compiled(subject, _parameter);
			var start = await boundary.Await();
			return new(query, start);
		}
	}
}