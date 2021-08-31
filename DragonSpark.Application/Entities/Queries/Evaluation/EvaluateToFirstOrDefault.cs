using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToFirstOrDefault<TIn, TContext, T> : Evaluate<TIn, T, T?> where TContext : DbContext
	{
		public EvaluateToFirstOrDefault(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToFirstOrDefault(IInvoke<TIn, T> invoke) : base(invoke, FirstOrDefault<T>.Default) {}
	}

	public class EvaluateToFirstOrDefault<TContext, T> : EvaluateToFirstOrDefault<None, TContext, T>
		where TContext : DbContext
	{
		public EvaluateToFirstOrDefault(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToFirstOrDefault(IInvoke<None, T> invoke) : base(invoke) {}
	}
}