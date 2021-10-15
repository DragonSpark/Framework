using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class Combine<T> : Combine<T, T>
{
	protected Combine(Expression<Func<DbContext, IQueryable<T>>> previous,
	                  Expression<Func<IQueryable<T>, IQueryable<T>>> select) : base(previous, select) {}

	protected Combine(Expression<Func<DbContext, IQueryable<T>>> previous,
	                  Expression<Func<DbContext, IQueryable<T>, IQueryable<T>>> select)
		: base(previous, select) {}

	protected Combine(Expression<Func<DbContext, None, IQueryable<T>>> previous,
	                  Expression<Func<DbContext, IQueryable<T>, IQueryable<T>>> instance)
		: base(previous, instance) {}

	public Combine(Expression<Func<DbContext, None, IQueryable<T>>> previous,
	               Expression<Func<IQueryable<T>, IQueryable<T>>> instance) : base(previous, instance) {}

	public Combine(Expression<Func<DbContext, None, IQueryable<T>>> previous,
	               Expression<Func<None, IQueryable<T>, IQueryable<T>>> instance) : base(previous, instance) {}

	protected Combine(Expression<Func<DbContext, None, IQueryable<T>>> previous,
	                  Expression<Func<DbContext, None, IQueryable<T>, IQueryable<T>>> instance)
		: base(previous, instance) {}
}

public class Combine<T, TTo> : Combine<None, T, TTo>, IQuery<TTo>
{
	protected Combine(Expression<Func<DbContext, IQueryable<T>>> previous,
	                  Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> instance)
		: base(previous.Then(), instance) {}

	protected Combine(Expression<Func<DbContext, IQueryable<T>>> previous,
	                  Expression<Func<IQueryable<T>, IQueryable<TTo>>> instance)
		: base(previous.Then(), instance) {}

	protected Combine(Expression<Func<DbContext, None, IQueryable<T>>> previous,
	                  Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> instance)
		: base(previous, instance) {}

	public Combine(Expression<Func<DbContext, None, IQueryable<T>>> previous,
	               Expression<Func<IQueryable<T>, IQueryable<TTo>>> instance)
		: base(previous, instance) {}

	public Combine(Expression<Func<DbContext, None, IQueryable<T>>> previous,
	               Expression<Func<None, IQueryable<T>, IQueryable<TTo>>> instance)
		: base(previous, instance) {}

	protected Combine(Expression<Func<DbContext, None, IQueryable<T>>> previous,
	                  Expression<Func<DbContext, None, IQueryable<T>, IQueryable<TTo>>> instance)
		: base(previous, instance) {}
}

public class Combine<TIn, T, TTo> : Query<TIn, TTo>
{
	public Combine(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	               Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> instance)
		: base((context, @in) => instance.Invoke(context, previous.Invoke(context, @in))) {}

	public Combine(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	               Expression<Func<IQueryable<T>, IQueryable<TTo>>> instance)
		: base((context, @in) => instance.Invoke(previous.Invoke(context, @in))) {}

	public Combine(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	               Expression<Func<TIn, IQueryable<T>, IQueryable<TTo>>> instance)
		: base((context, @in) => instance.Invoke(@in, previous.Invoke(context, @in))) {}

	public Combine(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	               Expression<Func<DbContext, TIn, IQueryable<T>, IQueryable<TTo>>> instance)
		: base((context, @in) => instance.Invoke(context, @in, previous.Invoke(context, @in))) {}
}