using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition
{
	public class Combine<T> : Combine<T, T>
	{
		protected Combine(Expression<Func<DbContext, IQueryable<T>>> previous,
		                  Expression<Func<IQueryable<T>, IQueryable<T>>> select) : base(previous, select) {}

		protected Combine(Expression<Func<DbContext, IQueryable<T>>> previous,
		                  Expression<Func<DbContext, IQueryable<T>, IQueryable<T>>> select)
			: base(previous, select) {}
	}

	public class Combine<T, TTo> : Combine<None, T, TTo>, IQuery<TTo>
	{
		public static implicit operator Expression<Func<DbContext, IQueryable<TTo>>>(Combine<T, TTo> instance)
		{
			var expression = instance.Get();
			return x => expression.Invoke(x, None.Default);
		}

		protected Combine(Expression<Func<DbContext, IQueryable<T>>> previous,
		                  Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> instance)
			: base((context, _) => previous.Invoke(context), instance) {}

		protected Combine(Expression<Func<DbContext, IQueryable<T>>> previous,
		                  Expression<Func<IQueryable<T>, IQueryable<TTo>>> instance)
			: base((context, _) => previous.Invoke(context), instance) {}
	}

	public class Combine<TIn, T, TTo> : Query<TIn, TTo>
	{
		protected Combine(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		                  Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> instance)
			: base((context, @in) => instance.Invoke(context, previous.Invoke(context, @in))) {}

		public Combine(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		               Expression<Func<IQueryable<T>, IQueryable<TTo>>> instance)
			: base((context, @in) => instance.Invoke(previous.Invoke(context, @in))) {}

		public Combine(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		               Expression<Func<TIn, IQueryable<T>, IQueryable<TTo>>> instance)
			: base((context, @in) => instance.Invoke(@in, previous.Invoke(context, @in))) {}

		protected Combine(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
		                  Expression<Func<DbContext, TIn, IQueryable<T>, IQueryable<TTo>>> instance)
			: base((context, @in) => instance.Invoke(context, @in, previous.Invoke(context, @in))) {}
	}

}