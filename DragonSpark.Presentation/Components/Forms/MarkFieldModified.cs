using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms;

public class MarkFieldModified : ComponentBase
{
	[Parameter]
	public bool Enabled { get; set; } = true;

	[Parameter]
	public string FieldName { get; set; } = null!;

	[CascadingParameter]
	EditContext EditContext { get; set; } = null!;

	protected override void OnInitialized()
	{
		base.OnInitialized();

		if (Enabled)
		{
			EditContext.NotifyFieldChanged(EditContext.Field(FieldName));
		}
	}
}