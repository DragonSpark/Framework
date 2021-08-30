using DragonSpark.Application.Entities.Queries;
using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Application.Entities.Queries.Evaluation;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;
using LinqKit;
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

		public ContextsAdapter<TAccept, T> Accept<TAccept>() => new(_subject);

		public ContextQueryAdapter<None, T, TElement> Use<TElement>() where TElement : class
			=> new(_subject, Set<TElement>.Default);

		public ContextQueryAdapter<TIn, T, TElement> Use<TIn, TElement>(IQuery<TIn, TElement> query)
			=> new(_subject, query);
	}

	public sealed class ContextsAdapter<TIn, T> where T : DbContext
	{
		readonly IContexts<T> _subject;

		public ContextsAdapter(IContexts<T> subject) => _subject = subject;

		public ContextQueryAdapter<TIn, T, TElement> Use<TElement>() where TElement : class
			=> new(_subject, Set<TIn, TElement>.Default);

		public ContextQueryAdapter<TIn, T, TElement> Use<TElement>(IQuery<TIn, TElement> query) => new(_subject, query);
	}

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

		public IntroducedQueryAdapter<TIn, TContext, T, TOther> Introduce<TOther>(IQuery<TIn, TOther> other)
			=> Introduce(other.Get());

		public IntroducedQueryAdapter<TIn, TContext, T, TOther>
			Introduce<TOther>(Expression<Func<DbContext, TIn, IQueryable<TOther>>> other)
			=> new(_contexts, _query, other);

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2> Introduce<T1, T2>(
			IQuery<T1> first, IQuery<T2> second)
			=> Introduce(first.Then().Without(), second.Then().Without());

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2> Introduce<T1, T2>(
			IQuery<T1> first, IQuery<TIn, T2> second)
			=> Introduce(first.Then().Without(), second.Get());

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2> Introduce<T1, T2>(
			IQuery<TIn, T1> first, IQuery<T2> second)
			=> Introduce(first.Get(), second.Then().Without());

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
			=> Introduce(first.Then().Without(), second.Then().Without(), third.Then().Without());

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3> Introduce<T1, T2, T3>(
			IQuery<TIn, T1> first, IQuery<TIn, T2> second, IQuery<T3> third)
			=> Introduce(first.Get(), second.Get(), third.Then().Without());

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3> Introduce<T1, T2, T3>(
			IQuery<TIn, T1> first, IQuery<T2> second, IQuery<T3> third)
			=> Introduce(first.Get(), second.Then().Without(), third.Then().Without());

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3> Introduce<T1, T2, T3>(
			IQuery<TIn, T1> first, IQuery<T2> second, IQuery<TIn, T3> third)
			=> Introduce(first.Get(), second.Then().Without(), third.Get());

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3> Introduce<T1, T2, T3>(
			IQuery<T1> first, IQuery<T2> second, IQuery<TIn, T3> third)
			=> Introduce(first.Then().Without(), second.Then().Without(), third.Get());

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3> Introduce<T1, T2, T3>(
			IQuery<T1> first, IQuery<TIn, T2> second, IQuery<T3> third)
			=> Introduce(first.Then().Without(), second.Get(), third.Then().Without());

		public IntroducedQueryAdapter<TIn, TContext, T, T1, T2, T3> Introduce<T1, T2, T3>(
			IQuery<T1> first, IQuery<TIn, T2> second, IQuery<TIn, T3> third)
			=> Introduce(first.Then().Without(), second.Get(), third.Get());

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

	public sealed class FormAdapter<TIn, TContext, T> where TContext : DbContext
	{
		readonly IContexts<TContext> _contexts;
		readonly IQuery<TIn, T>      _query;

		public FormAdapter(IContexts<TContext> contexts, IQuery<TIn, T> query)
		{
			_contexts = contexts;
			_query    = query;
		}

		public ISelecting<TIn, Array<T>> Array() => new EvaluateToArray<TIn, TContext, T>(_contexts, _query);

		public ISelecting<TIn, Lease<T>> Lease() => new EvaluateToLease<TIn, TContext, T>(_contexts, _query);

		public ISelecting<TIn, List<T>> List() => new EvaluateToList<TIn, TContext, T>(_contexts, _query);

		public ISelecting<TIn, Dictionary<TKey, T>> Dictionary<TKey>(Func<T, TKey> key) where TKey : notnull
			=> new EvaluateToMap<TIn, TContext, T, TKey>(_contexts, _query, key);

		public ISelecting<TIn, Dictionary<TKey, TValue>> Dictionary<TKey, TValue>(
			Func<T, TKey> key, Func<T, TValue> value)
			where TKey : notnull
			=> new EvaluateToMappedSelection<TIn, TContext, T, TKey, TValue>(_contexts, _query,
			                                                                 new ToDictionary<T, TKey,
				                                                                 TValue>(key, value));

		public ISelecting<TIn, T> Single() => new EvaluateToSingle<TIn, TContext, T>(_contexts, _query);

		public ISelecting<TIn, T?> SingleOrDefault()
			=> new EvaluateToSingleOrDefault<TIn, TContext, T>(_contexts, _query);

		public ISelecting<TIn, T> First() => new EvaluateToFirst<TIn, TContext, T>(_contexts, _query);

		public ISelecting<TIn, T?> FirstOrDefault()
			=> new EvaluateToFirstOrDefault<TIn, TContext, T>(_contexts, _query);

		public ISelecting<TIn, bool> Any() => new EvaluateToAny<TIn, TContext, T>(_contexts, _query);
	}
}