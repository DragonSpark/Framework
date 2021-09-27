
using DragonSpark.Application.Compose.Entities.Queries;
using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Editing;
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

		public ScopesComposer<None, TElement> Use<TElement>() where TElement : class
			=> new(Invocations(), Set<TElement>.Default);

		public ScopesComposer<TIn, TElement> Use<TIn, TElement>(IQuery<TIn, TElement> query)
			=> new(Invocations(), query);

		public IOperation<TIn> Form<TIn>(IFormed<TIn> formed) => new FormedAdapter<TIn, T>(_subject, formed);

		public IScopes Invocations() => new Scopes<T>(_subject);

		public IEdit<TIn, TOut> Edit<TIn, TOut>(ISelecting<TIn, TOut> select)
			=> new SelectForEdit<TIn, TOut>(Editing(), select);

		public IScopes Editing() => new AmbientAwareScopes(Invocations());
	}

	public sealed class ContextsComposer<TIn, T> where T : DbContext
	{
		readonly IContexts<T> _subject;

		public ContextsComposer(IContexts<T> subject) => _subject = subject;

		public ScopesComposer<TIn, TElement> Use<TElement>() where TElement : class
			=> new(new Scopes<T>(_subject), Set<TIn, TElement>.Default);

		public ScopesComposer<TIn, TElement> Use<TElement>(IQuery<TIn, TElement> query)
			=> new(new Scopes<T>(_subject), query);

		public IScopes Invocations() => new Scopes<T>(_subject);
	}
}