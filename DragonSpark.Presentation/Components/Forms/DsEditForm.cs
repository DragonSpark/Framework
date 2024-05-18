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
	Step? Step { get; set; }

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		base.BuildRenderTree(builder);

		if (Step is not null && EditContext is not null)
		{
			Step.RegisterEditFormAndContext(this);
		}
	}
}
