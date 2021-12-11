using Microsoft.AspNetCore.Components;

namespace DragonSpark.Syncfusion.Components;

public class GridTemplates : global::Syncfusion.Blazor.Grids.GridTemplates
{
	[Parameter]
	public RenderFragment LoadingTemplate { get; set; } = default!;
	

	protected override void OnInitialized()
	{
		base.OnInitialized();
		EmptyRecordTemplate = _ => LoadingTemplate;
	}
}