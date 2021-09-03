using DragonSpark.Application.Entities.Queries;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Compose.Entities
{
	class Class1 {}

	public sealed class ContextComposer
	{
		readonly DbContext _subject;

		public ContextComposer(DbContext subject) => _subject = subject;

		public ISelecting<TIn, TOut> Form<TIn, TOut>(IFormed<TIn, TOut> formed)
			=> new Application.Entities.Formed<TIn, TOut>(_subject, formed);
	}
}