using DragonSpark.Application.Entities.Queries.Composition;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities.Editing
{
	public class Editing<TIn, TContext, T> : Edit<TIn, T> where TContext : DbContext
	{
		protected Editing(IContexts<TContext> context, IQuery<TIn, T> query)
			: base(context.Then().Use(query).Edit.Single()) {}
	}
}