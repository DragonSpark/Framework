using DragonSpark.Application.Compose.Entities.Queries;
using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class ContextComposer
	{
		readonly DbContext _subject;

		public ContextComposer(DbContext subject) => _subject = subject;

		public IInvocations Invocations() => new ScopedInvocation(_subject);

		public InvocationComposer<None, TElement> Use<TElement>() where TElement : class
			=> new(Invocations(), Set<TElement>.Default);

		public InvocationComposer<TIn, TElement> Use<TIn, TElement>(IQuery<TIn, TElement> query)
			=> new(Invocations(), query);

		public ISelecting<TIn, TOut> Form<TIn, TOut>(IForming<TIn, TOut> forming)
			=> new Scoping<TIn, TOut>(_subject, forming);

		public IOperation<TIn> Form<TIn>(IFormed<TIn> formed) => new FormedAdapter<TIn>(_subject, formed);
	}
}