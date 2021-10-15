using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Runtime;

sealed class CompileAdapter<TIn, TOut> : ISelect<(DbContext, TIn), IQueryable<TOut>>
{
	readonly Func<DbContext, TIn, IQueryable<TOut>> _compiled;

	public CompileAdapter(Func<DbContext, TIn, IQueryable<TOut>> compiled) => _compiled = compiled;

	public IQueryable<TOut> Get((DbContext, TIn) parameter) => _compiled(parameter.Item1, parameter.Item2);
}