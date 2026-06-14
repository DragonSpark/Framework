using DragonSpark.Application.Components.Validation;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public abstract class FieldValidation<T> : ValidationComponent
{
	[Parameter]
	public required IValidateValue<T> Validator { get; set; }

	protected override bool Validate()
	{
		var value  = Identifier.GetValue<T>();
		var result = Validator.Get(value);
		return result;
	}
}