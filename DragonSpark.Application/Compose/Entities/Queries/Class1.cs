using DragonSpark.Application.Entities.Queries;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Compose.Entities.Queries
{
	class Class1 {}

	public sealed class ContextsAdapter<T> where T : DbContext
	{
		readonly IContexts<T> _subject;

		public ContextsAdapter(IContexts<T> subject) => _subject = subject;

		public ContextQueryAdapter<T, TElement> Use<TElement>() where TElement : class
			=> new(_subject, Set<TElement>.Default);

		public ContextQueryAdapter<T, TElement> Use<TElement>(IQuery<TElement> query) => new(_subject, query);
	}

	public sealed class ContextQueryAdapter<TContext, T> where TContext : DbContext
	{
		readonly IContexts<TContext> _contexts;
		readonly IQuery<T>           _query;

		public ContextQueryAdapter(IContexts<TContext> contexts, IQuery<T> query)
		{
			_contexts = contexts;
			_query    = query;
		}

		public ContextQueryAdapter<TContext, T> Where(Expression<Func<T, bool>> where)
			=> Next(new Where<T>(_query.Get(), where));

		public ContextQueryAdapter<TContext, TTo> Select<TTo>(Expression<Func<T, TTo>> select)
			=> Next(new Select<T, TTo>(_query.Get(), select));

		public ContextQueryAdapter<TContext, TTo> Select<TTo>(Expression<Func<IQueryable<T>, IQueryable<TTo>>> select)
			=> Next(new Combine<T, TTo>(_query.Get(), select));

		public ContextQueryAdapter<TContext, TTo> SelectMany<TTo>(Expression<Func<T, IEnumerable<TTo>>> select)
			=> Next(new SelectMany<T, TTo>(_query.Get(), select));

		public IntroducedQueryAdapter<TContext, T, TOther> Introduce<TOther>(IQuery<TOther> other)
			=> Introduce(other.Get());

		public IntroducedQueryAdapter<TContext, T, TOther>
			Introduce<TOther>(Expression<Func<DbContext, IQueryable<TOther>>> other)
			=> new(_contexts, _query, other);

		public IntroducedQueryAdapter<TContext, T, T1, T2> Introduce<T1, T2>(IQuery<T1> first, IQuery<T2> second)
			=> Introduce(first.Get(), second.Get());

		public IntroducedQueryAdapter<TContext, T, T1, T2>
			Introduce<T1, T2>(Expression<Func<DbContext, IQueryable<T1>>> first,
			                  Expression<Func<DbContext, IQueryable<T2>>> second)
			=> new(_contexts, _query, (first, second));

		public IntroducedQueryAdapter<TContext, T, T1, T2, T3> Introduce<T1, T2, T3>(IQuery<T1> first, IQuery<T2> second,
		                                                                         IQuery<T3> third)
			=> Introduce(first.Get(), second.Get(), third.Get());

		public IntroducedQueryAdapter<TContext, T, T1, T2, T3>
			Introduce<T1, T2, T3>(Expression<Func<DbContext, IQueryable<T1>>> first,
			                  Expression<Func<DbContext, IQueryable<T2>>> second,
							  Expression<Func<DbContext, IQueryable<T3>>> third)
			=> new(_contexts, _query, (first, second, third));

		ContextQueryAdapter<TContext, TTo> Next<TTo>(IQuery<TTo> next) => new(_contexts, next);

		public FormAdapter<TContext, T> To => new(_contexts, _query);
	}

	public sealed class IntroducedQueryAdapter<TContext, T, TOther> where TContext : DbContext
	{
		readonly IContexts<TContext>                             _contexts;
		readonly IQuery<T>                                       _query;
		readonly Expression<Func<DbContext, IQueryable<TOther>>> _other;

		public IntroducedQueryAdapter(IContexts<TContext> contexts, IQuery<T> query,
		                              Expression<Func<DbContext, IQueryable<TOther>>> other)
		{
			_contexts = contexts;
			_query    = query;
			_other    = other;
		}

		public ContextQueryAdapter<TContext, TTo> Select<TTo>(
			Expression<Func<IQueryable<T>, IQueryable<TOther>, IQueryable<TTo>>> @select)
			=> new(_contexts, new Introduce<T, TOther, TTo>(_query.Get(), _other, select));

		public ContextQueryAdapter<TContext, TTo> Select<TTo>(
			Expression<Func<DbContext, IQueryable<T>, IQueryable<TOther>, IQueryable<TTo>>> @select)
			=> new(_contexts, new Introduce<T, TOther, TTo>(_query.Get(), _other, select));
	}

	public sealed class IntroducedQueryAdapter<TContext, T, T1, T2> where TContext : DbContext
	{
		readonly IContexts<TContext>                                                                        _contexts;
		readonly IQuery<T>                                                                                  _query;
		readonly (Expression<Func<DbContext, IQueryable<T1>>>, Expression<Func<DbContext, IQueryable<T2>>>) _others;

		public IntroducedQueryAdapter(IContexts<TContext> contexts, IQuery<T> query,
		                              (Expression<Func<DbContext, IQueryable<T1>>>,
			                              Expression<Func<DbContext, IQueryable<T2>>>) others)
		{
			_contexts = contexts;
			_query    = query;
			_others   = others;
		}

		public ContextQueryAdapter<TContext, TTo> Select<TTo>(
			Expression<Func<IQueryable<T>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>> @select)
			=> new(_contexts, new Introduce<T, T1, T2, TTo>(_query.Get(), _others, select));

		public ContextQueryAdapter<TContext, TTo> Select<TTo>(
			Expression<Func<DbContext, IQueryable<T>, IQueryable<T1>, IQueryable<T2>, IQueryable<TTo>>> @select)
			=> new(_contexts, new Introduce<T, T1, T2, TTo>(_query.Get(), _others, select));
	}

	public sealed class IntroducedQueryAdapter<TContext, T, T1, T2, T3> where TContext : DbContext
	{
		readonly IContexts<TContext> _contexts;
		readonly IQuery<T>           _query;
		readonly (Expression<Func<DbContext, IQueryable<T1>>>, Expression<Func<DbContext, IQueryable<T2>>>,
			Expression<Func<DbContext, IQueryable<T3>>>) _others;

		public IntroducedQueryAdapter(IContexts<TContext> contexts, IQuery<T> query,
		                              (Expression<Func<DbContext, IQueryable<T1>>>,
			                              Expression<Func<DbContext, IQueryable<T2>>>,
			                              Expression<Func<DbContext, IQueryable<T3>>>) others)
		{
			_contexts = contexts;
			_query    = query;
			_others   = others;
		}

		public ContextQueryAdapter<TContext, TTo> Select<TTo>(
			Expression<Func<IQueryable<T>, IQueryable<T1>, IQueryable<T2>, IQueryable<T3>, IQueryable<TTo>>> @select)
			=> new(_contexts, new Introduce<T, T1, T2, T3, TTo>(_query.Get(), _others, select));

		public ContextQueryAdapter<TContext, TTo> Select<TTo>(
			Expression<Func<DbContext, IQueryable<T>, IQueryable<T1>, IQueryable<T2>, IQueryable<T3>, IQueryable<TTo>>>
				@select)
			=> new(_contexts, new Introduce<T, T1, T2, T3, TTo>(_query.Get(), _others, select));
	}

	public sealed class FormAdapter<TContext, T> where TContext : DbContext
	{
		readonly IContexts<TContext> _contexts;
		readonly IQuery<T>           _query;

		public FormAdapter(IContexts<TContext> contexts, IQuery<T> query)
		{
			_contexts = contexts;
			_query    = query;
		}

		public IResulting<Array<T>> Array() => new EvaluateToArray<TContext, T>(_contexts, _query);
	}
}