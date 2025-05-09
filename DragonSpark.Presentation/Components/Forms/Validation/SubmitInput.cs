using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public readonly record struct SubmitInput<T>(EditContext Context, T Subject, Switch Continue)
{
	public SubmitInput(EditContext Context, T Subject) : this(Context, Subject, true) {}
}