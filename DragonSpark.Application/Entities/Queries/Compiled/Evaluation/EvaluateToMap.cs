using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation
{
	public class EvaluateToMap<T, TKey> : EvaluateToMap<None, T, TKey>
		where TKey : notnull
	{
		/*
		public EvaluateToMap(IContexts<TContext> contexts, IQuery<None, T> query, Func<T, TKey> key)
			: base(contexts, query, key) {}
			*/

		public EvaluateToMap(IInvocations invocations, Expression<Func<DbContext, IQueryable<T>>> expression,
		                     Func<T, TKey> key) : base(invocations, expression.Then(), key) {}


		public EvaluateToMap(IInvocations invocations, Expression<Func<DbContext, None, IQueryable<T>>> expression,
		                     Func<T, TKey> key) : base(invocations, expression, key) {}

		public EvaluateToMap(IInvoke<None, T> invoke, IEvaluate<T, Dictionary<TKey, T>> evaluate)
			: base(invoke, evaluate) {}
	}

	public class EvaluateToMap<TIn, T, TKey> : Evaluate<TIn, T, Dictionary<TKey, T>>
		where TKey : notnull
	{
		/*
		public EvaluateToMap(IContexts<TContext> contexts, IQuery<TIn, T> query, Func<T, TKey> key)
			: this(contexts, query.Get(), key) {}
			*/

		public EvaluateToMap(IInvocations invocations, Expression<Func<DbContext, TIn, IQueryable<T>>> expression,
		                     Func<T, TKey> key)
			: this(new Invoke<TIn, T>(invocations, expression), new ToDictionary<T, TKey>(key)) {}

		public EvaluateToMap(IInvoke<TIn, T> invoke, IEvaluate<T, Dictionary<TKey, T>> evaluate)
			: base(invoke, evaluate) {}
	}
}