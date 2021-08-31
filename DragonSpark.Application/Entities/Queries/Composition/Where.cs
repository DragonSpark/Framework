using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition
{
	public class Where<TIn, T> : Combine<TIn, T, T>
	{
		public Where(Expression<Func<DbContext, TIn, IQueryable<T>>> previous, Expression<Func<T, bool>> where)
			: this(previous, (_, element) => where.Invoke(element)) {}

		public Where(Expression<Func<DbContext, TIn, IQueryable<T>>> previous, Expression<Func<TIn, T, bool>> where)
			: base(previous, (@in, q) => q.Where(x => where.Invoke(@in, x))) {}
	}

	public class Where<T> : Where<None, T>, IQuery<T>
	{
		public static implicit operator Expression<Func<DbContext, IQueryable<T>>>(Where<T> instance)
		{
			var expression = instance.Get();
			return x => expression.Invoke(x, None.Default);
		}

		public Where(Expression<Func<DbContext, IQueryable<T>>> previous, Expression<Func<T, bool>> where)
			: base((context, _) => previous.Invoke(context), where) {}
	}
}