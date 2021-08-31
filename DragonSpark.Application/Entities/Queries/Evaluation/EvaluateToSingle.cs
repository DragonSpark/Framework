using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToSingle<TIn, TContext, T> : Evaluate<TIn, T, T> where TContext : DbContext
	{
		public EvaluateToSingle(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToSingle(IInvoke<TIn, T> invoke) : base(invoke, Single<T>.Default) {}
	}

	public class EvaluateToSingle<TContext, T> : EvaluateToSingle<None, TContext, T> where TContext : DbContext
	{
		public EvaluateToSingle(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToSingle(IInvoke<None, T> invoke) : base(invoke) {}
	}
}