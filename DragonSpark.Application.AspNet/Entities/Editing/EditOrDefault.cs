using DragonSpark.Application.AspNet.Entities.Queries.Composition;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class EditOrDefault<TIn, T> : Edit<TIn, T?>
{
	protected EditOrDefault(IScopes scope, IQuery<TIn, T> query)
		: base(scope.Then().Use(query).Edit.SingleOrDefault()) {}
}