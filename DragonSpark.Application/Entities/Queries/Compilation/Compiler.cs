using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compilation
{
	sealed class Compiler<TIn, TOut> : Select<Expression<Func<DbContext, TIn, IQueryable<TOut>>>, IForm<TIn, TOut>>
	{
		public static Compiler<TIn, TOut> Default { get; } = new Compiler<TIn, TOut>();

		Compiler() : base(Start.An.Instance(ExpectedType<TIn, TOut>.Default.Then())
		                       .Select(Expand<TIn, TOut>.Default)
		                       .Select(Compile<TIn, TOut>.Default)) {}
	}
}