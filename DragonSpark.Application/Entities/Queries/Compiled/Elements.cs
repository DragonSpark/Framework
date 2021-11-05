using DragonSpark.Application.Entities.Queries.Composition;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled;

sealed class Elements<TIn, T> : DragonSpark.Model.Selection.Select<In<TIn>, IAsyncEnumerable<T>>, IElements<TIn, T>
{
	public Elements(IQuery<TIn, T> query) : this(query.Get()) {}

	public Elements(Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
		: base(ManyCompiler<TIn, T>.Default.Get(expression)) {}
}