using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public sealed class HasResults<T> : ICondition<IPages<T>>
{
	public static HasResults<T> Default { get; } = new();

	HasResults() {}

	public bool Get(IPages<T> parameter) => parameter.HasResults();
}