using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Queries.Compiled;
using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Application.Compose.Entities.Queries
{
	public sealed class InvocationComposer<TIn, T> : IResult<IInvoke<TIn, T>>
	{
		readonly IInvocations   _invocations;
		readonly IQuery<TIn, T> _query;

		public InvocationComposer(IInvocations invocations, IQuery<TIn, T> query)
		{
			_invocations = invocations;
			_query       = query;
		}

		public QueryInvocationComposer<TIn, T> To => new(Get());

		public EditInvocationComposer<TIn, T> Edit
			=> new(new InvocationComposer<TIn, T>(new AmbientAwareInvocations(_invocations), _query).Get());

		public InvocationComposer<TIn, TTo> Select<TTo>(
			Func<QueryComposer<TIn, T>, QueryComposer<TIn, TTo>> select)
			=> new(_invocations, select(new QueryComposer<TIn, T>(_query)).Get());

		public IRuntimeQuery<TIn, T> Compile() => new RuntimeQuery<TIn, T>(_invocations, _query);

		public IInvoke<TIn, T> Get() => new Invoke<TIn, T>(_invocations, _query.Get());
	}
}