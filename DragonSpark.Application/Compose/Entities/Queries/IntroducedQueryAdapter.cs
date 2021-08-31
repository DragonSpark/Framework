using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Queries;
using DragonSpark.Application.Entities.Queries.Composition;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Compose.Entities.Queries
{
	public sealed class IntroducedQueryAdapter<TIn, TContext, T, TOther> where TContext : DbContext
	{
		readonly IContexts<TContext>                                  _contexts;
		readonly IQuery<TIn, T>                                       _query;
		readonly Expression<Func<DbContext, TIn, IQueryable<TOther>>> _other;

		public IntroducedQueryAdapter(IContexts<TContext> contexts, IQuery<TIn, T> query,
		                              Expression<Func<DbContext, TIn, IQueryable<TOther>>> other)
		{
			_contexts = contexts;
			_query    = query;
			_other    = other;
		}

		public ContextQueryAdapter<TIn, TContext, TTo> Select<TTo>(
			Expression<Func<TIn, IQueryable<T>, IQueryable<TOther>, IQueryable<TTo>>> @select)
			=> new(_contexts, new Introduce<TIn, T, TOther, TTo>(_query.Get(), _other, select));

		public ContextQueryAdapter<TIn, TContext, TTo> Select<TTo>(
			Expression<Func<DbContext, TIn, IQueryable<T>, IQueryable<TOther>, IQueryable<TTo>>> @select)
			=> new(_contexts, new Introduce<TIn, T, TOther, TTo>(_query.Get(), _other, select));
	}

	public sealed class IntroducedQueryAdapter<TIn, TContext, T, T1, T2> where TContext : DbContext
	{
		readonly IContexts<TContext>                              _contexts;
		readonly IQuery<TIn, T>                                   _query;
		readonly Expression<Func<DbContext, TIn, IQueryable<T1>>> _first;
		readonly Expression<Func<DbContext, TIn, IQueryable<T2>>> _second;

		// ReSharper disable once TooManyDependencies
		public IntroducedQueryAdapter(IContexts<TContext> contexts, IQuery<TIn, T> query,
		                              Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
		                              Expression<Func<DbContext, TIn, IQueryable<T2>>> second)
		{
			_contexts = contexts;
			_query    = query;
			_first    = first;
			_second   = second;
		}

		public ContextQueryAdapter<TIn, TContext, TTo> Select<TTo>(
			Expression<Func<TIn, IQueryable<T>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>> @select)
			=> new(_contexts, new IntroduceTwo<TIn, T, T1, T2, TTo>(_query.Get(), _first, _second, select));

		public ContextQueryAdapter<TIn, TContext, TTo> Select<TTo>(
			Expression<Func<DbContext, TIn, IQueryable<T>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>> @select)
			=> new(_contexts, new IntroduceTwo<TIn, T, T1, T2, TTo>(_query.Get(), _first, _second, select));
	}

	public sealed class IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3> where TContext : DbContext
	{
		readonly IContexts<TContext>                              _contexts;
		readonly IQuery<TIn, T>                                   _query;
		readonly Expression<Func<DbContext, TIn, IQueryable<T1>>> _first;
		readonly Expression<Func<DbContext, TIn, IQueryable<T2>>> _second;
		readonly Expression<Func<DbContext, TIn, IQueryable<T3>>> _third;

		// ReSharper disable once TooManyDependencies
		public IntroducedQueryAdapter(IContexts<TContext> contexts, IQuery<TIn, T> query,
		                              Expression<Func<DbContext, TIn, IQueryable<T1>>> first,
		                              Expression<Func<DbContext, TIn, IQueryable<T2>>> second,
		                              Expression<Func<DbContext, TIn, IQueryable<T3>>> third)
		{
			_contexts = contexts;
			_query    = query;
			_first    = first;
			_second   = second;
			_third    = third;
		}

		public ContextQueryAdapter<TIn, TContext, TTo> Select<TTo>(
			Expression<Func<TIn, IQueryable<T>, IQueryable<T1>, IQueryable<T2>, IQueryable<T3>, IQueryable<TTo>>>
				@select)
			=> new(_contexts,
			       new IntroduceThree<TIn, T, T1, T2, T3, TTo>(_query.Get(), _first, _second, _third, select));

		public ContextQueryAdapter<TIn, TContext, TTo> Select<TTo>(
			Expression<Func<DbContext, TIn, IQueryable<T>, IQueryable<T1>, IQueryable<T2>, IQueryable<T3>,
					IQueryable<TTo>>>
				@select)
			=> new(_contexts,
			       new IntroduceThree<TIn, T, T1, T2, T3, TTo>(_query.Get(), _first, _second, _third, select));
	}
}