using DragonSpark.Application.AspNet.Compose.Entities.Queries;
using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Compose.Entities;

public sealed class ContextsComposer<T> where T : DbContext
{
	readonly INewContext<T> _subject;

	public ContextsComposer(INewContext<T> subject) => _subject = subject;

	public ScopesComposer<None, TElement> Use<TElement>() where TElement : class
		=> new(Scopes(), Set<TElement>.Default);

	public ScopesComposer<TIn, TElement> Use<TIn, TElement>(IQuery<TIn, TElement> query)
		=> new(Scopes(), query);

	public IScopes Scopes() => new Scopes<T>(_subject);
}