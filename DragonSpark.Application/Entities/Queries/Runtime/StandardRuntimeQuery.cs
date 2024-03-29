﻿using DragonSpark.Application.Entities.Queries.Composition;

namespace DragonSpark.Application.Entities.Queries.Runtime;

public class StandardRuntimeQuery<TIn, TOut> : RuntimeQuery<TIn, TOut>
{
	protected StandardRuntimeQuery(IStandardScopes scopes, IQuery<TIn, TOut> query) : base(scopes, query) {}
}

public class StandardRuntimeQuery<TOut> : RuntimeQuery<TOut>
{
	protected StandardRuntimeQuery(IStandardScopes scopes, IQuery<TOut> query) : base(scopes, query) {}
}