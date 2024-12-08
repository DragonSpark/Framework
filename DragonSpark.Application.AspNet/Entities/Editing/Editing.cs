using DragonSpark.Application.AspNet.Entities.Queries.Composition;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class Editing<TIn, T> : Edit<TIn, T>
{
	protected Editing(IScopes scope, IQuery<TIn, T> query) : base(scope.Then().Use(query).Edit.Single()) {}
}