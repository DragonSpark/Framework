using DragonSpark.Application.Entities.Queries;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System;
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
			=> Next(new Application.Entities.Queries.Selection<T, TTo>(_query.Get(), select));

		ContextQueryAdapter<TContext, TTo> Next<TTo>(IQuery<TTo> next) => new(_contexts, next);

		public FormAdapter<TContext, T> To => new(_contexts, _query);
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