using DragonSpark.Application.Compose.Entities.Queries;
using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Editing;
using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Model;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Compose.Entities
{
	public class ScopesComposer
	{
		readonly IScopes _subject;

		public ScopesComposer(IScopes subject) => _subject = subject;

		public IScopes WithAmbientLocation() => _subject as AmbientAwareScopes ?? new AmbientAwareScopes(_subject);

		public ScopesComposer<None, TElement> Use<TElement>() where TElement : class
			=> new(_subject, Set<TElement>.Default);


		public ScopesComposer<TIn, TElement> Use<TIn, TElement>(QueryComposer<TIn, TElement> query) => Use(query.Get());
		public ScopesComposer<TIn, TElement> Use<TIn, TElement>(IQuery<TIn, TElement> query) => new(_subject, query);

		public IEdit<TIn, TOut> Edit<TIn, TOut>(ISelecting<TIn, TOut> select)
			=> new SelectForEdit<TIn, TOut>(WithAmbientLocation(), select);
	}
}
