using DragonSpark.Application.Entities.Queries.Composition;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities.Queries.Runtime
{
	public class ContextRuntimeQuery<TIn, TContext, TOut> : RuntimeQuery<TIn, TOut> where TContext : DbContext
	{
		protected ContextRuntimeQuery(IContexts<TContext> contexts, IQuery<TIn, TOut> query)
			: base(new FactoryScopes<TContext>(contexts), query) {}
	}

	public class ContextRuntimeQuery<TContext, TOut> : RuntimeQuery<TOut> where TContext : DbContext
	{
		protected ContextRuntimeQuery(IContexts<TContext> contexts, IQuery<TOut> query)
			: base(new FactoryScopes<TContext>(contexts), query) {}
	}
}