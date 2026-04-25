using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Selection;

namespace DragonSpark.SyncfusionRendering.Queries.Data;

sealed class ComposeSort : ISelect<Syncfusion.Blazor.Data.Sort, Sort>
{
	public static ComposeSort Default { get; } = new();

	ComposeSort() {}

	public Sort Get(Syncfusion.Blazor.Data.Sort parameter)
		=> new(parameter.Name.Verify(), parameter.Direction.Verify());
}