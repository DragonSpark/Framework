using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public readonly record struct SubmittingInput(EditContext Context, Cancel Cancel)
{
	public SubmittingInput(EditContext context) : this(context, new()) {}
}