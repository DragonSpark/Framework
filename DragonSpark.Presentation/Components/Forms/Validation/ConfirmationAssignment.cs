using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed class ConfirmationAssignment : ComponentBase
{
	[Parameter]
	public ConfirmationVariable Subject { get; set; } = default!;

	[Parameter]
	public IResulting<bool> Assignment
	{
		get => _assignment;
		set
		{
			if (_assignment != value)
			{
				_assignment = value;
				Subject.Execute(_assignment);
			}
		}
	}

	IResulting<bool> _assignment = default!;
}