namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed record FieldValidationMessages(string Invalid, string Loading = "Validating this field... please wait",
											 string Error = "An exception occurred while validating this field")
{
	public static implicit operator FieldValidationMessages(string instance) => new(instance);
}