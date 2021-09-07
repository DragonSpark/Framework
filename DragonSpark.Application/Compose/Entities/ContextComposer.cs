using DragonSpark.Application.Entities;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class ContextComposer
	{
		readonly DbContext _subject;

		public ContextComposer(DbContext subject) => _subject = subject;

		public ISelecting<TIn, TOut> Form<TIn, TOut>(IForming<TIn, TOut> forming)
			=> new Scoping<TIn, TOut>(_subject, forming);

		public IOperation<TIn> Form<TIn>(IFormed<TIn> formed) => new FormedAdapter<TIn>(_subject, formed);
	}
}