using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToAny<TIn, TContext, T> : Evaluate<TIn, T, bool> where TContext : DbContext
	{
		public EvaluateToAny(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToAny(IInvoke<TIn, T> invoke) : base(invoke, Any<T>.Default) {}
	}

	public class EvaluateToAny<TContext, T> : EvaluateToAny<None, TContext, T> where TContext : DbContext
	{
		public EvaluateToAny(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToAny(IInvoke<None, T> invoke) : base(invoke) {}
	}
}