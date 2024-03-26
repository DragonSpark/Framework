using DragonSpark.Application.Entities.Queries.Composition;

namespace DragonSpark.Application.Entities.Editing;

public class EditFirstOrDefault<TIn, T> : Edit<TIn, T?>
{
	protected EditFirstOrDefault(IContexts context, IQuery<TIn, T> query)
		: base(context.Then().Use(query).Edit.FirstOrDefault()) {}
}