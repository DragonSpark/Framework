using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition
{
	public class Select<TIn, TFrom, TTo> : Combine<TIn, TFrom, TTo>
	{
		public Select(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> previous, Expression<Func<TFrom, TTo>> select)
			: this(previous, (_, from) => select.Invoke(from)) {}

		public Select(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> previous,
		              Expression<Func<TIn, TFrom, TTo>> select)
			: base(previous, (@in, q) => q.Select(x => select.Invoke(@in, x))) {}
	}

	public class Select<TFrom, TTo> : Select<None, TFrom, TTo>, IQuery<TTo>
	{
		public Select(Expression<Func<DbContext, IQueryable<TFrom>>> previous, Expression<Func<TFrom, TTo>> select)
			: base((context, _) => previous.Invoke(context), select) {}
	}
}