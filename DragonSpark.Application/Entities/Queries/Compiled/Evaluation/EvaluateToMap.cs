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
		protected EvaluateToMap(IInvocations invocations, Expression<Func<DbContext, IQueryable<T>>> expression,
		                        Func<T, TKey> key) : base(invocations, expression.Then(), key) {}


		protected EvaluateToMap(IInvocations invocations, Expression<Func<DbContext, None, IQueryable<T>>> expression,
		                        Func<T, TKey> key) : base(invocations, expression, key) {}

		protected EvaluateToMap(IInvoke<None, T> invoke, IEvaluate<T, Dictionary<TKey, T>> evaluate)
			: base(invoke, evaluate) {}
	}

	public class EvaluateToMap<TIn, T, TKey> : Evaluate<TIn, T, Dictionary<TKey, T>>
		where TKey : notnull
	{
		protected EvaluateToMap(IInvocations invocations, Expression<Func<DbContext, TIn, IQueryable<T>>> expression,
		                        Func<T, TKey> key)
			: this(new Invoke<TIn, T>(invocations, expression), new ToDictionary<T, TKey>(key)) {}

		protected EvaluateToMap(IInvoke<TIn, T> invoke, IEvaluate<T, Dictionary<TKey, T>> evaluate)
			: base(invoke, evaluate) {}
	}
}