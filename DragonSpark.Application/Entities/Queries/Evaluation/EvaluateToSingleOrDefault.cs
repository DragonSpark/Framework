using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToSingleOrDefault<TIn, TContext, T> : Evaluate<TIn, T, T?> where TContext : DbContext
	{
		public EvaluateToSingleOrDefault(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToSingleOrDefault(IInvoke<TIn, T> invoke) : base(invoke, SingleOrDefault<T>.Default) {}
	}

	public class EvaluateToSingleOrDefault<TContext, T> : EvaluateToSingleOrDefault<None, TContext, T>
		where TContext : DbContext
	{
		public EvaluateToSingleOrDefault(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToSingleOrDefault(IInvoke<None, T> invoke) : base(invoke) {}
	}
}