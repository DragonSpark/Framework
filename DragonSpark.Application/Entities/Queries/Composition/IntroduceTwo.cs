using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class IntroduceTwo<TIn, TFrom, T1, T2, TTo> : Query<TIn, TTo>
{
	// ReSharper disable once TooManyDependencies
	public IntroduceTwo(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> from,
	                    Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
	                    Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
	                    Expression<Func<DbContext, TIn, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>,
		                    IQueryable<TTo>>> select)
		: base((context, @in) => select.Invoke(context, @in, from.Invoke(context, @in), first.Invoke(context, @in),
		                                       second.Invoke(context, @in))) {}

	// ReSharper disable once TooManyDependencies
	public IntroduceTwo(Expression<Func<DbContext, TIn, IQueryable<TFrom>>> from,
	                    Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
	                    Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
	                    Expression<Func<TIn, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>>
		                    select)
		: base((context, @in) => select.Invoke(@in, from.Invoke(context, @in), first.Invoke(context, @in),
		                                       second.Invoke(context, @in))) {}
}

public class IntroduceTwo<TFrom, T1, T2, TTo> : IntroduceTwo<None, TFrom, T1, T2, TTo>, IQuery<TTo>
{
	// ReSharper disable once TooManyDependencies
	public IntroduceTwo(Expression<Func<DbContext, IQueryable<TFrom>>> from,
	                    Expression<Func<DbContext, IQueryable<T1>>> first,
	                    Expression<Func<DbContext, IQueryable<T2>>> second,
	                    Expression<Func<DbContext, IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>,
		                    IQueryable<TTo>>> select)
		: base((context, _) => from.Invoke(context), (context, _) => first.Invoke(context),
		       (context, _) => second.Invoke(context),
		       (context, _, f, t1, t2) => select.Invoke(context, f, t1, t2)) {}

	// ReSharper disable once TooManyDependencies
	public IntroduceTwo(Expression<Func<DbContext, IQueryable<TFrom>>> from,
	                    Expression<Func<DbContext, IQueryable<T1>>> first,
	                    Expression<Func<DbContext, IQueryable<T2>>> second,
	                    Expression<Func<IQueryable<TFrom>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>> select)
		: base((context, _) => from.Invoke(context), (context, _) => first.Invoke(context),
		       (context, _) => second.Invoke(context), (_, f, t1, t2) => select.Invoke(f, t1, t2)) {}
}