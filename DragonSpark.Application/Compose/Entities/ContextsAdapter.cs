using DragonSpark.Application.Compose.Entities.Queries;
using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Queries;
using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Compose.Entities
{
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
}