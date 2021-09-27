using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation
{
	public class EvaluateToMappedSelection<T, TKey, TValue> : EvaluateToMappedSelection<None, T, TKey, TValue>
		where TKey : notnull
	{
		// ReSharper disable once TooManyDependencies
		protected EvaluateToMappedSelection(IInvocations invocations,
		                                    Expression<Func<DbContext, IQueryable<T>>> expression, Func<T, TKey> key,
		                                    Func<T, TValue> value)
			: base(invocations, expression.Then(), key, value) {}

		// ReSharper disable once TooManyDependencies
		protected EvaluateToMappedSelection(IInvocations invocations,
		                                    Expression<Func<DbContext, None, IQueryable<T>>> expression,
		                                    Func<T, TKey> key,
		                                    Func<T, TValue> value)
			: base(invocations, expression, key, value) {}

		protected EvaluateToMappedSelection(IInvoke<None, T> invoke, Func<T, TKey> key, Func<T, TValue> value)
			: base(invoke, key, value) {}
	}

	public class EvaluateToMappedSelection<TIn, T, TKey, TValue> : Evaluate<TIn, T, Dictionary<TKey, TValue>>
		where TKey : notnull
	{
		// ReSharper disable once TooManyDependencies
		protected EvaluateToMappedSelection(IInvocations invocations,
		                                    Expression<Func<DbContext, TIn, IQueryable<T>>> expression,
		                                    Func<T, TKey> key, Func<T, TValue> value)
			: this(new Invoke<TIn, T>(invocations, expression), key, value) {}

		protected EvaluateToMappedSelection(IInvoke<TIn, T> invoke, Func<T, TKey> key, Func<T, TValue> value)
			: base(invoke, new ToDictionary<T, TKey, TValue>(key, value)) {}
	}
}