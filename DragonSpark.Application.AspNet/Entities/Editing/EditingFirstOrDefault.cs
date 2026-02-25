using DragonSpark.Application.AspNet.Entities.Queries.Composition;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class EditingFirstOrDefault<TIn, T> : Editing<TIn, T?>
{
	protected EditingFirstOrDefault(IScopes scope, IQuery<TIn, T> query)
		: base(scope.Then().Use(query).Edit.FirstOrDefault()) {}
}