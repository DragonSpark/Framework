using DragonSpark.Application.Entities.Queries;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using LinqKit;
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

		public EditInvocationComposer<TIn, T> Edit => new(Get());

		public InvocationComposer<TIn, TTo> Select<TTo>(
			Func<QueryComposer<TIn, T>, QueryComposer<TIn, TTo>> select)
			=> new(_invocations, select(new QueryComposer<TIn, T>(_query)).Get());

		public ISelect<TIn, IQueries<T>> Compile()
			=> new Compiled<TIn, T>(_invocations, _query.Get().Expand().Compile());

		public IInvoke<TIn, T> Get() => new Invoke<TIn, T>(_invocations, _query.Get());
	}
}