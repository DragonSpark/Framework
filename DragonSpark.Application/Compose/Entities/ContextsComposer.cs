
using DragonSpark.Application.Compose.Entities.Queries;
using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Compose.Entities;

public sealed class ContextsComposer<T> where T : DbContext
{
	readonly INewContext<T> _subject;

	public ContextsComposer(INewContext<T> subject) => _subject = subject;

	public ScopesComposer<None, TElement> Use<TElement>() where TElement : class
		=> new(Contexts(), Set<TElement>.Default);

	public ScopesComposer<TIn, TElement> Use<TIn, TElement>(IQuery<TIn, TElement> query)
		=> new(Contexts(), query);

	public IContexts Contexts() => new Contexts<T>(_subject);
}