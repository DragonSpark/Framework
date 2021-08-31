using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToMap<TIn, TContext, T, TKey> : Evaluate<TIn, T, Dictionary<TKey, T>>
		where TKey : notnull
		where TContext : DbContext
	{
		public EvaluateToMap(IContexts<TContext> contexts, IQuery<TIn, T> query, Func<T, TKey> key)
			: this(new Invoke<TContext, TIn, T>(contexts, query), new ToDictionary<T, TKey>(key)) {}

		public EvaluateToMap(IInvoke<TIn, T> invoke, IEvaluate<T, Dictionary<TKey, T>> evaluate)
			: base(invoke, evaluate) {}
	}

	public class EvaluateToMap<TContext, T, TKey> : EvaluateToMap<None, TContext, T, TKey>
		where TKey : notnull
		where TContext : DbContext
	{
		public EvaluateToMap(IContexts<TContext> contexts, IQuery<T> query, Func<T, TKey> key)
			: this(new Invoke<TContext, T>(contexts, query), new ToDictionary<T, TKey>(key)) {}

		public EvaluateToMap(IInvoke<None, T> invoke, IEvaluate<T, Dictionary<TKey, T>> evaluate)
			: base(invoke, evaluate) {}
	}
}