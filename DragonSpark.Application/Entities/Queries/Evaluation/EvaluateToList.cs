using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToList<TIn, TContext, T> : Evaluate<TIn, T, List<T>> where TContext : DbContext
	{
		public EvaluateToList(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToList(IInvoke<TIn, T> invoke) : base(invoke, ToList<T>.Default) {}
	}

	public class EvaluateToList<TContext, T> : EvaluateToList<None, TContext, T> where TContext : DbContext
	{
		public EvaluateToList(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToList(IInvoke<None, T> invoke) : base(invoke) {}
	}
}