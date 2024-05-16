using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Application.Components.Validation;

public readonly record struct NewValidationContext(
	FieldIdentifier Field,
	ObjectGraphValidator Validator,
	GraphValidationContext Context)
{
	public NewValidationContext(FieldIdentifier field, ObjectGraphValidator validator)
		: this(field, validator, new GraphValidationContext()) {}

	public NewValidationContext(object instance, ObjectGraphValidator validator, GraphValidationContext context)
		: this(new FieldIdentifier(instance, string.Empty), validator, context) {}
}