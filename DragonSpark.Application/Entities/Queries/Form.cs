using DragonSpark.Model.Selection;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries
{
	sealed class Form<TIn, T> : Select<In<TIn>, IAsyncEnumerable<T>>, IForm<TIn, T>
	{
		public Form(IQuery<TIn, T> query) : this(query.Get()) {}

		public Form(Expression<Func<DbContext, TIn, IQueryable<T>>> expression)
			: base(Compilation.Compile<TIn, T>.Default.Get(expression.Expand())) {}
	}
}