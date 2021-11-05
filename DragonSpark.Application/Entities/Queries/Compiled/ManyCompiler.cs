using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled;

sealed class ManyCompiler<TIn, TOut> : Select<Expression<Func<DbContext, TIn, IQueryable<TOut>>>, IElements<TIn, TOut>>
{
	public static ManyCompiler<TIn, TOut> Default { get; } = new ManyCompiler<TIn, TOut>();

	ManyCompiler() : base(Start.An.Instance(ExpectedType<TIn, TOut>.Default.Then())
	                           .Select(Expand<TIn, IQueryable<TOut>>.Default)
	                           .Select(ManyCompile<TIn, TOut>.Default)) {}
}