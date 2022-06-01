using DragonSpark.Application.Entities.Queries.Composition;

namespace DragonSpark.Application.Entities.Editing;

public class EditingOrDefault<TIn, T> : Edit<TIn, T?>
{
	protected EditingOrDefault(IScopes context, IQuery<TIn, T> query)
		: base(context.Then().Use(query).Edit.SingleOrDefault()) {}
}