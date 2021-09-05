using DragonSpark.Application.Compose.Entities.Queries;
using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Queries;
using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class ContextsComposer<T> where T : DbContext
	{
		readonly IContexts<T> _subject;

		public ContextsComposer(IContexts<T> subject) => _subject = subject;

		public ContextsComposer<TAccept, T> Accept<TAccept>() => new(_subject);

		public InvocationComposer<None, T, TElement> Use<TElement>() where TElement : class
			=> new(_subject, Set<TElement>.Default);

		public InvocationComposer<TIn, T, TElement> Use<TIn, TElement>(IQuery<TIn, TElement> query)
			=> new(_subject, query);

		public IEdit<TIn, TOut> Edit<TIn, TOut>(ISelecting<TIn, TOut> select)
			=> new StartEdit<TIn, T, TOut>(_subject, select);
	}

	public sealed class ContextsComposer<TIn, T> where T : DbContext
	{
		readonly IContexts<T> _subject;

		public ContextsComposer(IContexts<T> subject) => _subject = subject;

		public InvocationComposer<TIn, T, TElement> Use<TElement>() where TElement : class
			=> new(_subject, Set<TIn, TElement>.Default);

		public InvocationComposer<TIn, T, TElement> Use<TElement>(IQuery<TIn, TElement> query) => new(_subject, query);
	}
}