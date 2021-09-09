﻿using DragonSpark.Model;
using DragonSpark.Model.Sequences.Memory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	public class EvaluateToLease<T> : EvaluateToLease<None, T>
	{
		/*public EvaluateToLease(IContexts<TContext> contexts, IQuery<None, T> query) : base(contexts, query) {}*/

		public EvaluateToLease(IInvocations invocations, Expression<Func<DbContext, IQueryable<T>>> expression)
			: base(invocations, expression.Then()) {}

		public EvaluateToLease(IInvocations invocations,
		                       Expression<Func<DbContext, None, IQueryable<T>>> expression)
			: base(invocations, expression) {}

		public EvaluateToLease(IInvoke<None, T> invoke) : base(invoke) {}
	}

	public class EvaluateToLease<TIn, T> : Evaluate<TIn, T, Lease<T>>
	{
		/*public EvaluateToLease(IContexts<TContext> contexts, IQuery<TIn, T> query) : this(contexts, query.Get()) {}*/

		public EvaluateToLease(IInvocations invocations, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: this(new Invoke<TIn, T>(invocations, expression)) {}

		public EvaluateToLease(IInvoke<TIn, T> invoke) : base(invoke, ToLease<T>.Default) {}
	}
}