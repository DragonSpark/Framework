using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToMappedSelection<TIn, TContext, T, TKey, TValue>
		: Evaluate<TIn, T, Dictionary<TKey, TValue>>
		where TKey : notnull
		where TContext : DbContext
	{
		public EvaluateToMappedSelection(IContexts<TContext> contexts, IQuery<TIn, T> query,
		                                 IEvaluate<T, Dictionary<TKey, TValue>> evaluate)
			: this(new Invoke<TContext, TIn, T>(contexts, query), evaluate) {}

		public EvaluateToMappedSelection(IInvoke<TIn, T> invoke, IEvaluate<T, Dictionary<TKey, TValue>> evaluate)
			: base(invoke, evaluate) {}
	}

	public class EvaluateToMappedSelection<TContext, T, TKey, TValue>
		: EvaluateToMappedSelection<None, TContext, T, TKey, TValue>
		where TKey : notnull
		where TContext : DbContext
	{
		public EvaluateToMappedSelection(IContexts<TContext> contexts, IQuery<T> query,
		                                 IEvaluate<T, Dictionary<TKey, TValue>> evaluate)
			: this(new Invoke<TContext, T>(contexts, query), evaluate) {}

		public EvaluateToMappedSelection(IInvoke<None, T> invoke, IEvaluate<T, Dictionary<TKey, TValue>> evaluate)
			: base(invoke, evaluate) {}
	}
}