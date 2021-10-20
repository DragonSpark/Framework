using LinqKit;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class StartWhere<TIn, T> : Where<TIn, T> where T : class
{
	protected StartWhere(Expression<Func<T, bool>> where) : base(Set<TIn, T>.Default, where) {}

	protected StartWhere(Expression<Func<IQueryable<T>, IQueryable<T>>> previous,
	                     Expression<Func<TIn, T, bool>> where)
		: base(context => previous.Invoke(context.Set<T>()), where) {}

	protected StartWhere(Expression<Func<IQueryable<T>, IQueryable<T>>> previous,
	                     Expression<Func<T, bool>> where)
		: base(context => previous.Invoke(context.Set<T>()), where) {}

	protected StartWhere(Expression<Func<TIn, T, bool>> where) : base(Set<TIn, T>.Default, where) {}
}

public class StartWhere<T> : Where<T> where T : class
{
	protected StartWhere(Expression<Func<T, bool>> where) : base(Set<T>.Default, where) {}

	protected StartWhere(Expression<Func<IQueryable<T>, IQueryable<T>>> previous, Expression<Func<T, bool>> where) 
		: base(context => previous.Invoke(context.Set<T>()), where) {}
}