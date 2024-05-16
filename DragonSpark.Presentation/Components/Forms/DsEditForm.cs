using DragonSpark.Presentation.Components.Wizard;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace DragonSpark.Presentation.Components.Forms;

/// <summary>
/// Attribution: https://github.com/microsoft/fluentui-blazor/blob/dev/src/Core/Components/EditForm/FluentEditForm.cs
/// </summary>
public class DsEditForm : EditForm
{
	[CascadingParameter]
	public Step? WizardStep { get; set; }

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		base.BuildRenderTree(builder);

		if (WizardStep is not null && EditContext is not null)
		{
			WizardStep.RegisterEditFormAndContext(this, EditContext);
		}
	}
}
