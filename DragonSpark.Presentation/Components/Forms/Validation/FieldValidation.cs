using DragonSpark.Application.Components.Validation.Expressions;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public class FieldValidation<T> : ValidationComponent
{
	[Parameter]
	public IValidateValue<T> Validator { get; set; } = null!;

	protected override bool Validate() => Validator.Get(Identifier.GetValue<T>());
}