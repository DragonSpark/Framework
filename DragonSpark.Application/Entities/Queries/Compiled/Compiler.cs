using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled;

sealed class Compiler<TIn, TOut>
	: DragonSpark.Model.Selection.Select<Expression<Func<DbContext, TIn, TOut>>, IElement<TIn, TOut>>
{
	public static Compiler<TIn, TOut> Default { get; } = new();

	Compiler() : base(Start.An.Instance(Expand<TIn, TOut>.Default).Select(Compile<TIn, TOut>.Default)) { }
}