﻿using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition
{
	public class WhereSelect<TIn, T, TTo> : Combine<TIn, T, TTo>
	{
		protected WhereSelect(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		                      Expression<Func<T, bool>> where, Expression<Func<T, TTo>> select)
			: this(previous, (_, x) => where.Invoke(x), (_, x) => select.Invoke(x)) {}

		protected WhereSelect(Expression<Func<DbContext, TIn, IQueryable<T>>> previous, Expression<Func<T, bool>> where,
		                      Expression<Func<TIn, T, TTo>> select)
			: this(previous, (_, x) => where.Invoke(x), select) {}

		protected WhereSelect(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		                      Expression<Func<TIn, T, bool>> where, Expression<Func<T, TTo>> select)
			: this(previous, where, (_, x) => select.Invoke(x)) {}

		protected WhereSelect(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		                      Expression<Func<TIn, T, bool>> where, Expression<Func<TIn, T, TTo>> select)
			: base(previous, (@in, q) => q.Where(x => where.Invoke(@in, x)).Select(x => select.Invoke(@in, x))) {}
	}

	public class WhereSelect<T, TTo> : WhereSelect<None, T, TTo>, IQuery<TTo>
	{
		public static implicit operator Expression<Func<DbContext, IQueryable<TTo>>>(WhereSelect<T, TTo> instance)
		{
			var expression = instance.Get();
			return x => expression.Invoke(x, None.Default);
		}

		protected WhereSelect(Expression<Func<DbContext, IQueryable<T>>> previous, Expression<Func<T, bool>> where,
		                      Expression<Func<T, TTo>> select)
			: base((context, _) => previous.Invoke(context), where, select) {}
	}
}