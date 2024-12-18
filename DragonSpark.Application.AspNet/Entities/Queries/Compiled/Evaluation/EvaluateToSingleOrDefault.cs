﻿using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

public class EvaluateToSingleOrDefault<T> : EvaluateToSingleOrDefault<None, T>
{
	public EvaluateToSingleOrDefault(IScopes scopes, Expression<Func<DbContext, IQueryable<T>>> expression)
		: base(scopes, expression.Then()) {}

	public EvaluateToSingleOrDefault(IScopes scopes, Expression<Func<DbContext, None, IQueryable<T>>> expression)
		: base(scopes, expression) {}

	public EvaluateToSingleOrDefault(IReading<None, T> reading) : base(reading) {}
}

public class EvaluateToSingleOrDefault<TIn, T> : Evaluate<TIn, T, T?>
{
	public EvaluateToSingleOrDefault(IScopes scopes, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: this(new Reading<TIn, T>(scopes, (d, @in) => expression.Invoke(d, @in).Take(2))) {}

	protected EvaluateToSingleOrDefault(IReading<TIn, T> reading) : base(reading, ToSingleOrDefault<T>.Default) {}
}