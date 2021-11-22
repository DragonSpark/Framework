using DragonSpark.Application.Entities.Queries.Composition;

namespace DragonSpark.Application.Entities.Editing;

public class EditOrDefault<TIn, T> : Edit<TIn, T?>
{
	protected EditOrDefault(IScopes context, IQuery<TIn, T> query)
		: base(context.Then().Use(query).Edit.SingleOrDefault()) {}
}