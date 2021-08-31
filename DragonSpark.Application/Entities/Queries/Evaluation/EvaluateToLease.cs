using DragonSpark.Model;
using DragonSpark.Model.Sequences.Memory;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToLease<TIn, TContext, T> : Evaluate<TIn, T, Lease<T>> where TContext : DbContext
	{
		public EvaluateToLease(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToLease(IInvoke<TIn, T> invoke) : base(invoke, ToLease<T>.Default) {}
	}

	public class EvaluateToLease<TContext, T> : EvaluateToLease<None, TContext, T> where TContext : DbContext
	{
		public EvaluateToLease(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToLease(IInvoke<None, T> invoke) : base(invoke) {}
	}
}