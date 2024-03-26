using DragonSpark.Application.Entities.Queries.Composition;

namespace DragonSpark.Application.Entities.Editing;

public class EditFirstOrDefault<TIn, T> : Edit<TIn, T?>
{
	protected EditFirstOrDefault(IScopes scope, IQuery<TIn, T> query)
		: base(scope.Then().Use(query).Edit.FirstOrDefault()) {}
}