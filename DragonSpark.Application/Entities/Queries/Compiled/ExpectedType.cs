using DragonSpark.Model.Selection.Alterations;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled;

sealed class ExpectedType<TIn, TOut> : IAlteration<Expression<Func<DbContext, TIn, IQueryable<TOut>>>>
{
	public static ExpectedType<TIn, TOut> Default { get; } = new ExpectedType<TIn, TOut>();

	ExpectedType() {}

	public Expression<Func<DbContext, TIn, IQueryable<TOut>>> Get(
		Expression<Func<DbContext, TIn, IQueryable<TOut>>> parameter)
		=> (context, @in) => parameter.Invoke(context, @in).AsQueryable();
}