﻿using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToMappedSelection<T, TKey, TValue> : EvaluateToMappedSelection<None, T, TKey, TValue>
		where TKey : notnull
	{
		/*// ReSharper disable once TooManyDependencies
		public EvaluateToMappedSelection(IContexts<TContext> contexts, Func<T, TKey> key, IQuery<None, T> query,
		                                 Func<T, TValue> value)
			: base(contexts, key, query, value) {}
			*/

		// ReSharper disable once TooManyDependencies
		public EvaluateToMappedSelection(IInvocations invocations,
		                                 Expression<Func<DbContext, IQueryable<T>>> expression, Func<T, TKey> key,
		                                 Func<T, TValue> value) : base(invocations, expression.Then(), key, value) {}

		// ReSharper disable once TooManyDependencies
		public EvaluateToMappedSelection(IInvocations invocations,
		                                 Expression<Func<DbContext, None, IQueryable<T>>> expression, Func<T, TKey> key,
		                                 Func<T, TValue> value) : base(invocations, expression, key, value) {}

		public EvaluateToMappedSelection(IInvoke<None, T> invoke, Func<T, TKey> key, Func<T, TValue> value)
			: base(invoke, key, value) {}
	}

	public class EvaluateToMappedSelection<TIn, T, TKey, TValue> : Evaluate<TIn, T, Dictionary<TKey, TValue>>
		where TKey : notnull
	{
		/*
		// ReSharper disable once TooManyDependencies
		public EvaluateToMappedSelection(IContexts<TContext> contexts, Func<T, TKey> key, IQuery<TIn, T> query,
		                                 Func<T, TValue> value)
			: this(contexts, query.Get(), key, value) {}
			*/

		// ReSharper disable once TooManyDependencies
		public EvaluateToMappedSelection(IInvocations invocations,
		                                 Expression<Func<DbContext, TIn, IQueryable<T>>> expression, Func<T, TKey> key,
		                                 Func<T, TValue> value)
			: this(new Invoke<TIn, T>(invocations, expression), key, value) {}

		public EvaluateToMappedSelection(IInvoke<TIn, T> invoke, Func<T, TKey> key, Func<T, TValue> value)
			: base(invoke, new ToDictionary<T, TKey, TValue>(key, value)) {}
	}
}