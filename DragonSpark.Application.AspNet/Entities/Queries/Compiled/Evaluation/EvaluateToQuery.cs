using DragonSpark.Model.Selection;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

public class EvaluateToQuery<TIn, T> : ISelect<TIn, IQueryable<T>>
{
	readonly DbContext                                       _context;
	readonly Expression<Func<DbContext, TIn, IQueryable<T>>> _expression;

	protected EvaluateToQuery(DbContext context, Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
	{
		_context    = context;
		_expression = expression;
	}

	public IQueryable<T> Get(TIn parameter) => _expression.Invoke(_context, parameter).AsExpandable();
}