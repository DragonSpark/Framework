using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components.Content.Templates;
using DragonSpark.Presentation.Components.Forms.Validation;
using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms;

public class EditFormContainerBase<T> : InteractiveComponentBase<T>
{
	[Parameter]
	public required bool EnableChangeMonitor { get; set; } = true;

	[Parameter]
	public required RenderFragment LoadingTemplate { get; set; } = DefaultLoadingTemplate.Default;

	[Parameter]
	public string EditText { get; set; } = "Edit...";

	[Parameter]
	public string CancelText { get; set; } = "Cancel";

	[Parameter]
	public string SaveText { get; set; } = "Save";

	[Parameter]
	public string ButtonCssClass { get; set; } = "button";

	[Parameter]
	public string SaveButtonCssClass { get; set; } = "primary button";

	[Parameter]
	public EventCallback<SubmittingInput> Submitting { get; set; }

	[Parameter]
	public EventCallback<EditContext> Submitted { get; set; }

	[Parameter]
	public EventCallback<IMutable<T?>> Editing { get; set; }

	[Parameter]
	public EventCallback Canceled { get; set; }

	[Parameter]
	public bool AllowEdit { get; set; } = true;
}