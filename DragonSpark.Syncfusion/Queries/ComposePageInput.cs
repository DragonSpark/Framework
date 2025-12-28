using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Selection;
using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering.Queries;

public sealed class ComposePageInput : ISelect<DataManagerRequest, PageInput>
{
	public static ComposePageInput Default { get; } = new();

	ComposePageInput() {}

	public PageInput Get(DataManagerRequest parameter) => new SyncfusionPageInput(parameter);
}