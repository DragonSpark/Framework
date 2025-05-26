using DragonSpark.Model.Operations.Results;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed class ConfirmationAssignment : ComponentBase
{
	[Parameter]
	public ConfirmationVariable Subject { get; set; } = null!;

	[Parameter]
	public IResulting<bool> Assignment
	{
		get;
		set
		{
			if (field != value)
			{
				field = value;
				Subject.Execute(field);
			}
		}
	} = null!;
}