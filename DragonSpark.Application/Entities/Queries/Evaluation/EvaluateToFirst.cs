using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToFirst<TIn, TContext, T> : Evaluate<TIn, T, T> where TContext : DbContext
	{
		public EvaluateToFirst(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToFirst(IInvoke<TIn, T> invoke) : base(invoke, First<T>.Default) {}
	}

	public class EvaluateToFirst<TContext, T> : EvaluateToFirst<None, TContext, T> where TContext : DbContext
	{
		public EvaluateToFirst(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToFirst(IInvoke<None, T> invoke) : base(invoke) {}
	}
}