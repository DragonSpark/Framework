using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

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

public class IntroduceThree<TFrom, T1, T2, T3, TTo> : IntroduceThree<None, TFrom, T1, T2, T3, TTo>, IQuery<TTo>
{
	// ReSharper disable once TooManyDependencies
	public IntroduceThree(Expression<Func<DbContext, IQueryable<TFrom>>> from,
	                      Expression<Func<DbContext, IQueryable<T1>>> first,
	                      Expression<Func<DbContext, IQueryable<T2>>> second,
	                      Expression<Func<DbContext, IQueryable<T3>>> third,
	                      Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>,
		                      IQueryable<T3>, IQueryable<TTo>>> select)
		: base((context, _) => from.Invoke(context), (context, _) => first.Invoke(context),
		       (context, _) => second.Invoke(context),
		       (context, _) => third.Invoke(context),
		       (context, _, f, t1, t2, t3) => select.Invoke(context, f, t1, t2, t3)) {}

	// ReSharper disable once TooManyDependencies
	public IntroduceThree(Expression<Func<DbContext, IQueryable<TFrom>>> from,
	                      Expression<Func<DbContext, IQueryable<T1>>> first,
	                      Expression<Func<DbContext, IQueryable<T2>>> second,
	                      Expression<Func<DbContext, IQueryable<T3>>> third,
	                      Expression<Func<IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<T3>,
		                      IQueryable<TTo>>> select)
		: base((context, _) => from.Invoke(context), (context, _) => first.Invoke(context),
		       (context, _) => second.Invoke(context), (context, _) => third.Invoke(context),
		       (_, f, t1, t2, t3) => select.Invoke(f, t1, t2, t3)) {}
}