using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public sealed class HasResults : ICondition<bool?>
{
	public static HasResults Default { get; } = new();

	HasResults() {}

	public bool Get(bool? parameter) => parameter is null || parameter.Value;
}