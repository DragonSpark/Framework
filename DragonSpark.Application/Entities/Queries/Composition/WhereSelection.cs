using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class WhereSelection<TIn, T, TTo> : Combine<TIn, T, TTo>
{
	protected WhereSelection(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	                         Expression<Func<T, bool>> where,
	                         Expression<Func<IQueryable<T>, IQueryable<TTo>>> select)
		: this(previous, (_, x) => where.Invoke(x), (_, x) => select.Invoke(x)) {}

	protected WhereSelection(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	                         Expression<Func<T, bool>> where,
	                         Expression<Func<TIn, IQueryable<T>, IQueryable<TTo>>> select)
		: this(previous, (_, x) => where.Invoke(x), select) {}

	protected WhereSelection(IQuery<T> previous, Expression<Func<TIn, T, bool>> where,
	                         Expression<Func<IQueryable<T>, IQueryable<TTo>>> select)
		: this((context, _) => previous.Get().Invoke(context, None.Default), where, (_, x) => select.Invoke(x)) {}

	protected WhereSelection(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	                         Expression<Func<TIn, T, bool>> where,
	                         Expression<Func<IQueryable<T>, IQueryable<TTo>>> select)
		: this(previous, where, (_, x) => select.Invoke(x)) {}

	protected WhereSelection(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	                         Expression<Func<TIn, T, bool>> where,
	                         Expression<Func<TIn, IQueryable<T>, IQueryable<TTo>>> select)
		: base(previous, (@in, q) => select.Invoke(@in, q.Where(x => where.Invoke(@in, x)))) {}

	protected WhereSelection(IQuery<T> previous, Expression<Func<TIn, T, bool>> where,
	                         Expression<Func<DbContext, TIn, IQueryable<T>, IQueryable<TTo>>> select)
		: this((context, _) => previous.Get().Invoke(context, None.Default), @where, @select) {}

	protected WhereSelection(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	                         Expression<Func<TIn, T, bool>> where,
	                         Expression<Func<DbContext, TIn, IQueryable<T>, IQueryable<TTo>>> select)
		: base(previous, (context, @in, q) => select.Invoke(context, @in, q.Where(x => where.Invoke(@in, x)))) {}
}

public class WhereSelection<T, TTo> : WhereSelection<None, T, TTo>
{
	protected WhereSelection(Expression<Func<DbContext, IQueryable<T>>> previous,
	                         Expression<Func<T, bool>> where,
	                         Expression<Func<IQueryable<T>, IQueryable<TTo>>> select)
		: base(previous.Then(), where, select) {}

	protected WhereSelection(Expression<Func<DbContext, IQueryable<T>>> previous,
	                         Expression<Func<T, bool>> where,
	                         Expression<Func<None, IQueryable<T>, IQueryable<TTo>>> select)
		: base(previous.Then(), where, select) {}

	protected WhereSelection(Expression<Func<DbContext, IQueryable<T>>> previous,
	                         Expression<Func<None, T, bool>> where,
	                         Expression<Func<IQueryable<T>, IQueryable<TTo>>> select)
		: base(previous.Then(), where, select) {}

	protected WhereSelection(Expression<Func<DbContext, IQueryable<T>>> previous,
	                         Expression<Func<None, T, bool>> where,
	                         Expression<Func<None, IQueryable<T>, IQueryable<TTo>>> select) :
		base(previous.Then(), where, select) {}
}