using DragonSpark.Model;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToArray<TIn, TContext, T> : Evaluate<TIn, T, Array<T>> where TContext : DbContext
	{
		public EvaluateToArray(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(new Invoke<TContext, TIn, T>(contexts, query)) {}

		public EvaluateToArray(IInvoke<TIn, T> invoke) : base(invoke, ToArray<T>.Default) {}
	}

	public class EvaluateToArray<TContext, T> : EvaluateToArray<None, TContext, T> where TContext : DbContext
	{
		public EvaluateToArray(IContexts<TContext> contexts, IQuery<T> query)
			: this(new Invoke<TContext, T>(contexts, query)) {}

		public EvaluateToArray(IInvoke<None, T> invoke) : base(invoke) {}
	}
}