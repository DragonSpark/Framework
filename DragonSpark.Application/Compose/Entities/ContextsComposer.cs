
using DragonSpark.Application.Compose.Entities.Queries;
using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Compose.Entities;

public sealed class ContextsComposer<T> where T : DbContext
{
	readonly IContexts<T> _subject;

	public ContextsComposer(IContexts<T> subject) => _subject = subject;

	public ScopesComposer<None, TElement> Use<TElement>() where TElement : class
		=> new(Scopes(), Set<TElement>.Default);

	public ScopesComposer<TIn, TElement> Use<TIn, TElement>(IQuery<TIn, TElement> query)
		=> new(Scopes(), query);

	public IStandardScopes Scopes() => new StandardScopes<T>(_subject);
}