using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Queries.Compiled;
using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.AspNet.Compose.Entities.Queries;

public sealed class ScopesComposer<TIn, T> : IResult<IReading<TIn, T>>
{
	readonly IScopes        _scopes;
	readonly IQuery<TIn, T> _query;

	public ScopesComposer(IScopes scopes, IQuery<TIn, T> query)
	{
		_scopes = scopes;
		_query  = query;
	}

	public QueryInvocationComposer<TIn, T> To => new(_scopes, _query);

	public EditInvocationComposer<TIn, T> Edit => new(Get());

	public IReading<TIn, T> Get() => new Reading<TIn, T>(_scopes, _query.Get());
}