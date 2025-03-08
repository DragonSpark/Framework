using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public class FieldValidationMessage<T> : ValidationComponent
{
	[Parameter]
	public IValidationMessage<T> Validator { get; set; } = null!;

	protected override bool Validate()
	{
		var message = Validator.Get(Identifier.GetValue<T>());
		var result  = string.IsNullOrEmpty(message);
		if (!result)
		{
			ErrorMessage = message.Verify();
		}
		return result;
	}
}