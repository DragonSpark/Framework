﻿using DragonSpark.Model;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Composition;

public class WhereSelect<TIn, T, TTo> : Combine<TIn, T, TTo> where T : class
{
	protected WhereSelect(IQuery<T> previous, Expression<Func<T, bool>> where, Expression<Func<T, TTo>> select)
		: this((context, _) => previous.Get().Invoke(context, None.Default), (_, x) => where.Invoke(x),
		       (_, x) => select.Invoke(x)) {}

	protected WhereSelect(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	                      Expression<Func<T, bool>> where, Expression<Func<T, TTo>> select)
		: this(previous, (_, x) => where.Invoke(x), (_, x) => select.Invoke(x)) {}

	protected WhereSelect(Expression<Func<DbContext, TIn, IQueryable<T>>> previous, Expression<Func<T, bool>> where,
	                      Expression<Func<TIn, T, TTo>> select)
		: this(previous, (_, x) => where.Invoke(x), select) {}

	protected WhereSelect(IQuery<T> query, Expression<Func<TIn, T, bool>> where, Expression<Func<T, TTo>> select)
		: this((context, _) => query.Get().Invoke(context, None.Default), @where, @select) {}

	protected WhereSelect(IQuery<T> query, Expression<Func<TIn, T, bool>> where,
	                      Expression<Func<DbContext, T, TTo>> select)
		: this((context, _) => query.Get().Invoke(context, None.Default), @where, (d, _, x) => select.Invoke(d, x)) {}

	protected WhereSelect(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	                      Expression<Func<TIn, T, bool>> where, Expression<Func<T, TTo>> select)
		: this(previous, where, (_, x) => select.Invoke(x)) {}

	protected WhereSelect(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	                      Expression<Func<DbContext, TIn, T, bool>> where, Expression<Func<T, TTo>> select)
		: this(previous, where, (_, _, x) => select.Invoke(x)) {}

	protected WhereSelect(IQuery<T> previous, Expression<Func<TIn, T, bool>> where,
	                      Expression<Func<TIn, T, TTo>> select)
		: this((context, _) => previous.Get().Invoke(context, None.Default), @where, @select) {}

	protected WhereSelect(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	                      Expression<Func<TIn, T, bool>> where, Expression<Func<TIn, T, TTo>> select)
		: base(previous, (@in, q) => q.Where(x => where.Invoke(@in, x)).Select(x => select.Invoke(@in, x))) {}

	protected WhereSelect(Expression<Func<IQueryable<T>, IQueryable<T>>> previous,
	                      Expression<Func<DbContext, TIn, T, bool>> where,
	                      Expression<Func<T, TTo>> select)
		: this((d, @in) => previous.Invoke(d.Set<T>()), where, select) {}

	protected WhereSelect(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	                      Expression<Func<TIn, T, bool>> where,
	                      Expression<Func<DbContext, TIn, T, TTo>> select)
		: this(previous, (_, @in, x) => where.Invoke(@in, x), select) {}

	protected WhereSelect(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	                      Expression<Func<T, bool>> where,
	                      Expression<Func<DbContext, T, TTo>> select)
		: base(previous, (d, @in, q) => q.Where(x => where.Invoke(x)).Select(x => select.Invoke(d, x))) {}

	protected WhereSelect(Expression<Func<DbContext, TIn, IQueryable<T>>> previous,
	                      Expression<Func<DbContext, TIn, T, bool>> where,
	                      Expression<Func<DbContext, TIn, T, TTo>> select)
		: base(previous,
		       (context, @in, q)
			       => q.Where(x => where.Invoke(context, @in, x)).Select(x => select.Invoke(context, @in, x))) {}
}

public class WhereSelect<T, TTo> : WhereSelect<None, T, TTo>, IQuery<TTo> where T : class
{
	protected WhereSelect(IQuery<T> previous, Expression<Func<T, bool>> where, Expression<Func<T, TTo>> select)
		: this(previous.Then(), where, select) {}

	protected WhereSelect(Expression<Func<DbContext, IQueryable<T>>> previous, Expression<Func<T, bool>> where,
	                      Expression<Func<T, TTo>> select)
		: base((context, _) => previous.Invoke(context), where, select) {}

	protected WhereSelect(Expression<Func<DbContext, IQueryable<T>>> previous, Expression<Func<T, bool>> where,
	                      Expression<Func<DbContext, T, TTo>> select)
		: base((context, _) => previous.Invoke(context), where,  select) {}
}