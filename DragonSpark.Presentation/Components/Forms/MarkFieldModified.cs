using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms
{
	public class MarkFieldModified : ComponentBase
	{
		[Parameter]
		public string FieldName { get; set; } = default!;

		[CascadingParameter]
		EditContext EditContext { get; set; } = default!;

		protected override void OnInitialized()
		{
			base.OnInitialized();
			EditContext.NotifyFieldChanged(EditContext.Field(FieldName));
		}
	}
}