using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Dialogs;

public class ConfirmResultComponent : ComponentBase
{
	[Parameter]
	public string PrimaryClass { get; set; } = null!;

	[Parameter]
	public string SecondaryClass { get; set; } = null!;

	[Parameter]
	public string PrimaryText { get; set; } = "OK";

	[Parameter]
	public string SecondaryText { get; set; } = "Cancel";

	[Parameter]
	public string ContentClass { get; set; } = "ds-dialog-content card-body";

	[Parameter]
	public Func<ValueTask<bool>> Confirmation { get; set; } = () => true.ToOperation();

	[Parameter]
	public RenderFragment ChildContent { get; set; } = null!;

}