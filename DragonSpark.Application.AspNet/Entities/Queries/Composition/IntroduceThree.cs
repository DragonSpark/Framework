using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Queries.Composition;

public class IntroduceThree<TIn, TFrom, T1, T2, T3, TTo> : Query<TIn, TTo>
{
	// ReSharper disable once TooManyDependencies
	public IntroduceThree(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> from,
	                      Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
	                      Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
	                      Expression<Func<DbContext, TIn, IQueryable<T3>>> third,
	                      Expression<Func<DbContext, TIn, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>,
		                      IQueryable<T3>,
		                      IQueryable<TTo>>> select)
		: base((context, @in) => select.Invoke(context, @in, from.Invoke(context, @in), first.Invoke(context, @in),
		                                       second.Invoke(context, @in), third.Invoke(context, @in))) {}

	// ReSharper disable once TooManyDependencies
	public IntroduceThree(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> from,
	                      Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
	                      Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
	                      Expression<Func<DbContext, TIn, IQueryable<T3>>> third,
	                      Expression<Func<TIn, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<T3>,
		                      IQueryable<TTo>>> select)
		: base((context, @in) => select.Invoke(@in, from.Invoke(context, @in), first.Invoke(context, @in),
		                                       second.Invoke(context, @in), third.Invoke(context, @in))) {}
}