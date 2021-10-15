using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class StartInput<TIn, T> : StartInput<TIn, T, T> where T : class
{
	protected StartInput(Expression<Func<DbContext, IQueryable<T>, IQueryable<T>>> instance)
		: base(instance) {}

	protected StartInput(Expression<Func<IQueryable<T>, IQueryable<T>>> instance) : base(instance) {}

	protected StartInput(Expression<Func<TIn, IQueryable<T>, IQueryable<T>>> instance) : base(instance) {}

	protected StartInput(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	                     Expression<Func<DbContext, TIn, IQueryable<T>, IQueryable<T>>> instance)
		: base(previous, instance) {}
}

public class StartInput<TIn, T, TTo> : Combine<TIn, T, TTo> where T : class
{
	protected StartInput(Expression<Func<DbContext, IQueryable<T>, IQueryable<TTo>>> instance)
		: base(Set<TIn, T>.Default, instance) {}

	protected StartInput(Expression<Func<IQueryable<T>, IQueryable<TTo>>> instance)
		: base(Set<TIn, T>.Default, instance) {}

	protected StartInput(Expression<Func<TIn, IQueryable<T>, IQueryable<TTo>>> instance)
		: base(Set<TIn, T>.Default, instance) {}

	protected StartInput(Expression<Func<DbContext, TIn, IQueryable<T>, IQueryable<TTo>>> instance)
		: base(Set<TIn, T>.Default, instance) {}

	protected StartInput(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	                     Expression<Func<DbContext, TIn, IQueryable<T>, IQueryable<TTo>>> instance)
		: base(previous, instance) {}
}