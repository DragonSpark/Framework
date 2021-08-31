using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToMap<TContext, T, TKey> : EvaluateToMap<None, TContext, T, TKey>
		where TKey : notnull
		where TContext : DbContext
	{
		/*
		public EvaluateToMap(IContexts<TContext> contexts, IQuery<None, T> query, Func<T, TKey> key)
			: base(contexts, query, key) {}
			*/

		public EvaluateToMap(IContexts<TContext> contexts, Expression<Func<DbContext, IQueryable<T>>> expression,
		                     Func<T, TKey> key) : base(contexts, expression.Then(), key) {}


		public EvaluateToMap(IContexts<TContext> contexts, Expression<Func<DbContext, None, IQueryable<T>>> expression,
		                     Func<T, TKey> key) : base(contexts, expression, key) {}

		public EvaluateToMap(IInvoke<None, T> invoke, IEvaluate<T, Dictionary<TKey, T>> evaluate)
			: base(invoke, evaluate) {}
	}

	public class EvaluateToMap<TIn, TContext, T, TKey> : Evaluate<TIn, T, Dictionary<TKey, T>>
		where TKey : notnull
		where TContext : DbContext
	{
		/*
		public EvaluateToMap(IContexts<TContext> contexts, IQuery<TIn, T> query, Func<T, TKey> key)
			: this(contexts, query.Get(), key) {}
			*/

		public EvaluateToMap(IContexts<TContext> contexts, Expression<Func<DbContext, TIn, IQueryable<T>>> expression,
		                     Func<T, TKey> key)
			: this(new Invoke<TContext, TIn, T>(contexts, expression), new ToDictionary<T, TKey>(key)) {}

		public EvaluateToMap(IInvoke<TIn, T> invoke, IEvaluate<T, Dictionary<TKey, T>> evaluate)
			: base(invoke, evaluate) {}
	}
}