using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class StartIntroduceTwo<TIn, TFrom, T1, T2, TTo> : IntroduceTwo<TIn, TFrom, T1, T2, TTo> where TFrom : class
{
	// ReSharper disable once TooManyDependencies
	protected StartIntroduceTwo(Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
	                            Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
	                            Expression<Func<DbContext, TIn, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>,
		                            IQueryable<TTo>>> select)
		: base(Set<TIn, TFrom>.Default, first, second, select) {}

	// ReSharper disable once TooManyDependencies
	protected StartIntroduceTwo(Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
	                            Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
	                            Expression<Func<TIn, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>,
		                            IQueryable<TTo>>> select)
		: base(Set<TIn, TFrom>.Default, first, second, select) {}
}

public class StartIntroduceTwo<TFrom, T1, T2, TTo> : IntroduceTwo<TFrom, T1, T2, TTo> where TFrom : class
{
	// ReSharper disable once TooManyDependencies
	protected StartIntroduceTwo(
		Expression<Func<DbContext, IQueryable<T1>>> first, Expression<Func<DbContext, IQueryable<T2>>> second,
		Expression<Func<IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>> select)
		: base(Set<TFrom>.Default.Then(), first, second, select) {}

	// ReSharper disable once TooManyDependencies
	protected StartIntroduceTwo(
		Expression<Func<DbContext, IQueryable<T1>>> first, Expression<Func<DbContext, IQueryable<T2>>> second,
		Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>> select)
		: base(Set<TFrom>.Default.Then(), first, second, select) {}
}