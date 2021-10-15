using DragonSpark.Application.Entities.Queries.Composition;

namespace DragonSpark.Application.Entities.Editing;

public class Editing<TIn, T> : Edit<TIn, T>
{
	protected Editing(IScopes context, IQuery<TIn, T> query) : base(context.Then().Use(query).Edit.Single()) {}
}