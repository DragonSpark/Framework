﻿using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Queries;
using DragonSpark.Application.Entities.Queries.Composition;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Compose.Entities.Queries
{
	public sealed class ContextQueryAdapter<TIn, TContext, T> where TContext : DbContext
	{
		readonly IContexts<TContext> _contexts;
		readonly IQuery<TIn, T>      _query;

		public ContextQueryAdapter(IContexts<TContext> contexts, IQuery<TIn, T> query)
		{
			_contexts = contexts;
			_query    = query;
		}

		public ContextQueryAdapter<TIn, TContext, T> Where(Expression<Func<T, bool>> where)
			=> Next(new Where<TIn, T>(_query.Get(), where));

		public ContextQueryAdapter<TIn, TContext, T> Where(Expression<Func<TIn, T, bool>> where)
			=> Next(new Where<TIn, T>(_query.Get(), where));

		public ContextQueryAdapter<TIn, TContext, TTo> Select<TTo>(Expression<Func<T, TTo>> select)
			=> Next(new Select<TIn, T, TTo>(_query.Get(), select));

		public ContextQueryAdapter<TIn, TContext, TTo> Select<TTo>(Expression<Func<TIn, T, TTo>> select)
			=> Next(new Select<TIn, T, TTo>(_query.Get(), select));

		public ContextQueryAdapter<TIn, TContext, TTo>
			Select<TTo>(Expression<Func<IQueryable<T>, IQueryable<TTo>>> select)
			=> Next(new Combine<TIn, T, TTo>(_query.Get(), select));

		public ContextQueryAdapter<TIn, TContext, TTo>
			Select<TTo>(Expression<Func<TIn, IQueryable<T>, IQueryable<TTo>>> select)
			=> Next(new Combine<TIn, T, TTo>(_query.Get(), select));

		public ContextQueryAdapter<TIn, TContext, TTo> SelectMany<TTo>(Expression<Func<T, IEnumerable<TTo>>> select)
			=> Next(new SelectMany<TIn, T, TTo>(_query.Get(), select));

		public ContextQueryAdapter<TIn, TContext, TTo> SelectMany<TTo>(
			Expression<Func<TIn, T, IEnumerable<TTo>>> select)
			=> Next(new SelectMany<TIn, T, TTo>(_query.Get(), select));

		public IntroducedQueryAdapter<TIn, TContext, T, TOther> Introduce<TOther>(IQuery<TOther> other)
			=> Introduce(other.Then().Remove());

		public IntroducedQueryAdapter<TIn, TContext, T, TOther> Introduce<TOther>(IQuery<TIn, TOther> other)
			=> Introduce(other.Get());

		public IntroducedQueryAdapter<TIn, TContext, T, TOther>
			Introduce<TOther>(Expression<Func<DbContext, IQueryable<TOther>>> other)
			=> new(_contexts, _query, (context, _) => other.Invoke(context));

		public IntroducedQueryAdapter<TIn, TContext, T, TOther>
			Introduce<TOther>(Expression<Func<DbContext, TIn, IQueryable<TOther>>> other)
			=> new(_contexts, _query, other);

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2> Introduce<T1, T2>(
			IQuery<T1> first, IQuery<T2> second)
			=> Introduce(first.Then().Remove(), second.Then().Remove());

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2> Introduce<T1, T2>(
			IQuery<T1> first, IQuery<TIn, T2> second)
			=> Introduce(first.Then().Remove(), second.Get());

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2> Introduce<T1, T2>(
			IQuery<TIn, T1> first, IQuery<T2> second)
			=> Introduce(first.Get(), second.Then().Remove());

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2> Introduce<T1, T2>(IQuery<TIn, T1> first,
		                                                                          IQuery<TIn, T2> second)
			=> Introduce(first.Get(), second.Get());


		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2>
			Introduce<T1, T2>(Expression<Func<DbContext, IQueryable<T1>>> first,
			                  Expression<Func<DbContext, IQueryable<T2>>> second)
			=> Introduce((context, _) => first.Invoke(context), (context, _) => second.Invoke(context));

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2>
			Introduce<T1, T2>(Expression<Func<DbContext, IQueryable<T1>>> first,
			                  Expression<Func<DbContext, TIn, IQueryable<T2>>> second)
			=> Introduce((context, _) => first.Invoke(context), second);

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2>
			Introduce<T1, T2>(Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
			                  Expression<Func<DbContext, IQueryable<T2>>> second)
			=> Introduce(first, (context, _) => second.Invoke(context));

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2>
			Introduce<T1, T2>(Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
			                  Expression<Func<DbContext, TIn, IQueryable<T2>>> second)
			=> new(_contexts, _query, first, second);

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3> Introduce<T1, T2, T3>(
			IQuery<T1> first, IQuery<T2> second, IQuery<T3> third)
			=> Introduce(first.Then().Remove(), second.Then().Remove(), third.Then().Remove());

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3> Introduce<T1, T2, T3>(
			IQuery<TIn, T1> first, IQuery<TIn, T2> second, IQuery<T3> third)
			=> Introduce(first.Get(), second.Get(), third.Then().Remove());

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3> Introduce<T1, T2, T3>(
			IQuery<TIn, T1> first, IQuery<T2> second, IQuery<T3> third)
			=> Introduce(first.Get(), second.Then().Remove(), third.Then().Remove());

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3> Introduce<T1, T2, T3>(
			IQuery<TIn, T1> first, IQuery<T2> second, IQuery<TIn, T3> third)
			=> Introduce(first.Get(), second.Then().Remove(), third.Get());

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3> Introduce<T1, T2, T3>(
			IQuery<T1> first, IQuery<T2> second, IQuery<TIn, T3> third)
			=> Introduce(first.Then().Remove(), second.Then().Remove(), third.Get());

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3> Introduce<T1, T2, T3>(
			IQuery<T1> first, IQuery<TIn, T2> second, IQuery<T3> third)
			=> Introduce(first.Then().Remove(), second.Get(), third.Then().Remove());

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3> Introduce<T1, T2, T3>(
			IQuery<T1> first, IQuery<TIn, T2> second, IQuery<TIn, T3> third)
			=> Introduce(first.Then().Remove(), second.Get(), third.Get());

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3> Introduce<T1, T2, T3>(
			IQuery<TIn, T1> first, IQuery<TIn, T2> second, IQuery<TIn, T3> third)
			=> Introduce(first.Get(), second.Get(), third.Get());

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3>
			Introduce<T1, T2, T3>(Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
			                      Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
			                      Expression<Func<DbContext, IQueryable<T3>>> third)
			=> Introduce(first, second, (context, _) => third.Invoke(context));

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3>
			Introduce<T1, T2, T3>(Expression<Func<DbContext, IQueryable<T1>>> first,
			                      Expression<Func<DbContext, IQueryable<T2>>> second,
			                      Expression<Func<DbContext, IQueryable<T3>>> third)
			=> Introduce(first, second, (context, _) => third.Invoke(context));

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3>
			Introduce<T1, T2, T3>(Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
			                      Expression<Func<DbContext, IQueryable<T2>>> second,
			                      Expression<Func<DbContext, IQueryable<T3>>> third)
			=> Introduce(first, second, (context, _) => third.Invoke(context));

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3>
			Introduce<T1, T2, T3>(Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
			                      Expression<Func<DbContext, IQueryable<T2>>> second,
			                      Expression<Func<DbContext, TIn, IQueryable<T3>>> third)
			=> Introduce(first, (context, _) => second.Invoke(context), third);

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3>
			Introduce<T1, T2, T3>(Expression<Func<DbContext, IQueryable<T1>>> first,
			                      Expression<Func<DbContext, IQueryable<T2>>> second,
			                      Expression<Func<DbContext, TIn, IQueryable<T3>>> third)
			=> Introduce(first, (context, _) => second.Invoke(context), third);

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3>
			Introduce<T1, T2, T3>(Expression<Func<DbContext, IQueryable<T1>>> first,
			                      Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
			                      Expression<Func<DbContext, IQueryable<T3>>> third)
			=> Introduce(first, second, (context, _) => third.Invoke(context));

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3>
			Introduce<T1, T2, T3>(Expression<Func<DbContext, IQueryable<T1>>> first,
			                      Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
			                      Expression<Func<DbContext, TIn, IQueryable<T3>>> third)
			=> Introduce((context, _) => first.Invoke(context), second, third);

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3>
			Introduce<T1, T2, T3>(Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
			                      Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
			                      Expression<Func<DbContext, TIn, IQueryable<T3>>> third)
			=> new(_contexts, _query, first, second, third);

		ContextQueryAdapter<TIn, TContext, TTo> Next<TTo>(IQuery<TIn, TTo> next) => new(_contexts, next);

		public FormAdapter<TIn, TContext, T> To => new(_contexts, _query);
	}
}