
using DragonSpark.Application.Compose.Entities.Queries;
using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Editing;
using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Application.Entities.Queries.Runtime;
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

		public InvocationComposer<None, TElement> Use<TElement>() where TElement : class
			=> new(Invocations(), Set<TElement>.Default);

		public InvocationComposer<TIn, TElement> Use<TIn, TElement>(IQuery<TIn, TElement> query)
			=> new(Invocations(), query);

		public IEdit<TIn, TOut> Edit<TIn, TOut>(ISelecting<TIn, TOut> select)
			=> new SelectedEdit<TIn, T, TOut>(_subject, select);

		public IInvocations Invocations() => new Invocations<T>(_subject);
	}

	public sealed class ContextsComposer<TIn, T> where T : DbContext
	{
		readonly IContexts<T> _subject;

		public ContextsComposer(IContexts<T> subject) => _subject = subject;

		public InvocationComposer<TIn, TElement> Use<TElement>() where TElement : class
			=> new(new Invocations<T>(_subject), Set<TIn, TElement>.Default);

		public InvocationComposer<TIn, TElement> Use<TElement>(IQuery<TIn, TElement> query)
			=> new(new Invocations<T>(_subject), query);

		public IInvocations Invocations() => new Invocations<T>(_subject);
	}
}