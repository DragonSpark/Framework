using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries
{
	class Class1 {}

	sealed class Compiled<TContext, TIn, TOut> : ISelect<TIn, IQueryable<TOut>> where TContext : DbContext
	{
		readonly IContexts<TContext>                    _contexts;
		readonly Func<DbContext, TIn, IQueryable<TOut>> _compiled;

		public Compiled(IContexts<TContext> contexts, Func<DbContext, TIn, IQueryable<TOut>> compiled)
		{
			_contexts      = contexts;
			_compiled = compiled;
		}

		public IQueryable<TOut> Get(TIn parameter) => _compiled(_contexts.Get(), parameter);
	}
}