using DragonSpark.Application.Entities.Queries.Composition;

namespace DragonSpark.Application.Entities.Queries.Runtime;

public class StandardRuntimeQuery<TIn, TOut> : RuntimeQuery<TIn, TOut>
{
	protected StandardRuntimeQuery(IContexts contexts, IQuery<TIn, TOut> query) : base(contexts, query) {}
}

public class StandardRuntimeQuery<TOut> : RuntimeQuery<TOut>
{
	protected StandardRuntimeQuery(IContexts contexts, IQuery<TOut> query) : base(contexts, query) {}
}