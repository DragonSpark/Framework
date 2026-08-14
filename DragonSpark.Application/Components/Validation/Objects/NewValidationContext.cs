namespace DragonSpark.Application.Components.Validation.Objects;

public readonly record struct NewValidationContext(
	FieldDescriptor Field,
	ObjectGraphValidator Validator,
	GraphValidationContext Context)
{
	public NewValidationContext(FieldDescriptor field, ObjectGraphValidator validator)
		: this(field, validator, new GraphValidationContext()) {}

	public NewValidationContext(object instance, ObjectGraphValidator validator, GraphValidationContext context)
		: this(new(instance, string.Empty), validator, context) {}
}