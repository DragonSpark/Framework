using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms
{
	public class EditContextPublisher : ComponentBase
	{
		[Parameter]
		public EventCallback<EditContext> Updated { get; set; }

		[CascadingParameter]
		EditContext? EditContext { get; set; }

		public override async Task SetParametersAsync(ParameterView parameters)
		{
			var change = parameters.DidParameterChange(nameof(EditContext), EditContext);
			
			await base.SetParametersAsync(parameters);

			if (change)
			{
				await Updated.InvokeAsync(EditContext);
			}
		}
	}
}
