using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Queries.Compiled;
using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Application.Compose.Entities.Queries
{
	public sealed class ScopesComposer<TIn, T> : IResult<IReading<TIn, T>>
	{
		readonly IScopes        _scopes;
		readonly IQuery<TIn, T> _query;

		public ScopesComposer(IScopes scopes, IQuery<TIn, T> query)
		{
			_scopes = scopes;
			_query  = query;
		}

		public QueryInvocationComposer<TIn, T> To => new(Get());

		public EditInvocationComposer<TIn, T> Edit
			=> new(new ScopesComposer<TIn, T>(_scopes.Then().WithAmbientLocation(), _query).Get());

		public ScopesComposer<TIn, TTo> Select<TTo>(
			Func<QueryComposer<TIn, T>, QueryComposer<TIn, TTo>> select)
			=> new(_scopes, select(new QueryComposer<TIn, T>(_query)).Get());

		public IRuntimeQuery<TIn, T> Compile() => new RuntimeQuery<TIn, T>(_scopes, _query);

		public IReading<TIn, T> Get() => new Reading<TIn, T>(_scopes, _query.Get());
	}
}