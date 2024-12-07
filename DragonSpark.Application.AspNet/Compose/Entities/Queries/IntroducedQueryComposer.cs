using DragonSpark.Application.Entities.Queries.Composition;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Compose.Entities.Queries;

public sealed class IntroducedQueryComposer<TIn, T, TOther>
{
	readonly IQuery<TIn, T>                                       _query;
	readonly Expression<Func<DbContext, TIn, IQueryable<TOther>>> _other;

	public IntroducedQueryComposer(IQuery<TIn, T> query, Expression<Func<DbContext, TIn, IQueryable<TOther>>> other)
	{
		_query = query;
		_other = other;
	}

	public QueryComposer<TIn, TTo> Select<TTo>(
		Expression<Func<TIn, IQueryable<T>, IQueryable<TOther>, IQueryable<TTo>>> select)
		=> new(new Introduce<TIn, T, TOther, TTo>(_query.Get(), _other, select));

	public QueryComposer<TIn, TTo> Select<TTo>(
		Expression<Func<DbContext, TIn, IQueryable<T>, IQueryable<TOther>, IQueryable<TTo>>> select)
		=> new(new Introduce<TIn, T, TOther, TTo>(_query.Get(), _other, select));
}

public sealed class IntroducedQueryComposer<TIn, T, T1, T2>
{
	readonly IQuery<TIn, T>                                   _query;
	readonly Expression<Func<DbContext, TIn, IQueryable<T1>>> _first;
	readonly Expression<Func<DbContext, TIn, IQueryable<T2>>> _second;

	public IntroducedQueryComposer(IQuery<TIn, T> query, Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
	                               Expression<Func<DbContext, TIn, IQueryable<T2>>> second)
	{
		_query  = query;
		_first  = first;
		_second = second;
	}

	public QueryComposer<TIn, TTo> Select<TTo>(
		Expression<Func<TIn, IQueryable<T>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>> select)
		=> new(new IntroduceTwo<TIn, T, T1, T2, TTo>(_query.Get(), _first, _second, select));

	public QueryComposer<TIn, TTo> Select<TTo>(
		Expression<Func<DbContext, TIn, IQueryable<T>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>> select)
		=> new(new IntroduceTwo<TIn, T, T1, T2, TTo>(_query.Get(), _first, _second, select));
}

public sealed class IntroducedQueryComposer<TIn, T, T1, T2, T3>
{
	readonly IQuery<TIn, T> _query;
	readonly (Expression<Func<DbContext, TIn, IQueryable<T1>>>, Expression<Func<DbContext, TIn, IQueryable<T2>>>,
		Expression<Func<DbContext, TIn, IQueryable<T3>>>) _others;

	public IntroducedQueryComposer(IQuery<TIn, T> query,
	                               (Expression<Func<DbContext, TIn, IQueryable<T1>>>,
		                               Expression<Func<DbContext, TIn, IQueryable<T2>>>,
		                               Expression<Func<DbContext, TIn, IQueryable<T3>>>) others)
	{
		_query  = query;
		_others = others;
	}

	public QueryComposer<TIn, TTo> Select<TTo>(
		Expression<Func<TIn, IQueryable<T>, IQueryable<T1>, IQueryable<T2>, IQueryable<T3>, IQueryable<TTo>>>
			select)
	{
		var (first, second, third) = _others;
		return new(new IntroduceThree<TIn, T, T1, T2, T3, TTo>(_query.Get(), first, second, third, select));
	}

	public QueryComposer<TIn, TTo> Select<TTo>(
		Expression<Func<DbContext, TIn, IQueryable<T>, IQueryable<T1>, IQueryable<T2>, IQueryable<T3>,
				IQueryable<TTo>>>
			select)
	{
		var (first, second, third) = _others;
		return new(new IntroduceThree<TIn, T, T1, T2, T3, TTo>(_query.Get(), first, second, third, select));
	}
}