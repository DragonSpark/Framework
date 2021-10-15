using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class StartIntroduceThree<TIn, TFrom, T1, T2, T3, TTo> : IntroduceThree<TIn, TFrom, T1, T2, T3, TTo>
	where TFrom : class
{
	// ReSharper disable once TooManyDependencies
	protected StartIntroduceThree(Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
	                              Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
	                              Expression<Func<DbContext, TIn, IQueryable<T3>>> third,
	                              Expression<Func<DbContext, TIn, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>,
		                              IQueryable<T3>, IQueryable<TTo>>> select)
		: base(Set<TIn, TFrom>.Default, first, second, third, select) {}

	// ReSharper disable once TooManyDependencies
	protected StartIntroduceThree(Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
	                              Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
	                              Expression<Func<DbContext, TIn, IQueryable<T3>>> third,
	                              Expression<Func<TIn, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>,
		                              IQueryable<T3>, IQueryable<TTo>>> select)
		: base(Set<TIn, TFrom>.Default, first, second, third, select) {}
}

public class StartIntroduceThree<TFrom, T1, T2, T3, TTo> : IntroduceThree<TFrom, T1, T2, T3, TTo>
	where TFrom : class
{
	// ReSharper disable once TooManyDependencies
	protected StartIntroduceThree(
		Expression<Func<DbContext, IQueryable<T1>>> first, Expression<Func<DbContext, IQueryable<T2>>> second,
		Expression<Func<DbContext, IQueryable<T3>>> third,
		Expression<Func<IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<T3>, IQueryable<TTo>>> select)
		: base(Set<TFrom>.Default.Then(), first, second, third, select) {}

	// ReSharper disable once TooManyDependencies
	protected StartIntroduceThree(
		Expression<Func<DbContext, IQueryable<T1>>> first, Expression<Func<DbContext, IQueryable<T2>>> second,
		Expression<Func<DbContext, IQueryable<T3>>> third,
		Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<T3>,
			IQueryable<TTo>>> select)
		: base(Set<TFrom>.Default.Then(), first, second, third, select) {}
}