using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Queries.Compiled;
using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Compose.Entities.Queries;

public sealed class ScopesComposer<TIn, T> : IResult<IReading<TIn, T>>
{
	readonly IContexts        _contexts;
	readonly IQuery<TIn, T> _query;

	public ScopesComposer(IContexts contexts, IQuery<TIn, T> query)
	{
		_contexts = contexts;
		_query  = query;
	}

	public QueryInvocationComposer<TIn, T> To => new(_contexts, _query);

	public EditInvocationComposer<TIn, T> Edit => new(Get());

	public IReading<TIn, T> Get() => new Reading<TIn, T>(_contexts, _query.Get());
}