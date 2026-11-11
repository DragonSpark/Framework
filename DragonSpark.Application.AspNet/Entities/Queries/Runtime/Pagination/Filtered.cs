using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

sealed class Filtered : Condition<PageInput>, IFiltered
{
	public static Filtered Default { get; } = new();

	Filtered() : base(x => x.Filter.IsAssigned()) {}
}