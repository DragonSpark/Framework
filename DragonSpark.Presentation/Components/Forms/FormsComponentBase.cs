using DragonSpark.Presentation.Components.Forms.Validation;
using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms;

public class FormsComponentBase : InteractiveComponentBase
{
	DragonSpark.Model.Results.Switch _submitting = null!;

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		var change = parameters.DidParameterChange(nameof(EditContext), EditContext);
		await base.SetParametersAsync(parameters);
		if (change)
		{
			_submitting = Submitting.Default.Get(EditContext);
		}
	}

	[CascadingParameter]
	protected EditContext EditContext { get; set; } = null!;

	[CascadingParameter]
	protected FieldRegistry Fields { get; set; } = null!;

	protected override bool ShouldRender() => base.ShouldRender() && !_submitting;
}
