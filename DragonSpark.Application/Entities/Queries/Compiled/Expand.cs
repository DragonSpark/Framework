using DragonSpark.Model.Selection.Alterations;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled;

sealed class Expand<TIn, TOut> : IAlteration<Expression<Func<DbContext, TIn, TOut>>>
{
	public static Expand<TIn, TOut> Default { get; } = new Expand<TIn, TOut>();

	Expand() {}

	public Expression<Func<DbContext, TIn, TOut>> Get(Expression<Func<DbContext, TIn, TOut>> parameter)
		=> parameter.Expand();
}