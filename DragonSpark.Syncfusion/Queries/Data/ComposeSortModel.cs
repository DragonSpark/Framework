using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Selection;

namespace DragonSpark.SyncfusionRendering.Queries.Data;

sealed class ComposeSortModel : ISelect<Sort, Syncfusion.Blazor.Data.Sort>
{
	public static ComposeSortModel Default { get; } = new();

	ComposeSortModel() {}

	public Syncfusion.Blazor.Data.Sort Get(Sort parameter)
	{
		var (name, direction) = parameter;
		return new() { Name = name, Direction = direction };
	}
}