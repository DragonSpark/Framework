using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Application.Components.Validation;

public readonly struct NewValidationContext
{
	public NewValidationContext(FieldIdentifier field, ObjectGraphValidator validator)
		: this(field, validator, new GraphValidationContext()) {}

	public NewValidationContext(object instance, ObjectGraphValidator validator, GraphValidationContext context)
		: this(new FieldIdentifier(instance, string.Empty), validator, context) {}

	public NewValidationContext(FieldIdentifier field, ObjectGraphValidator validator, GraphValidationContext context)
	{
		Field     = field;
		Validator = validator;
		Context   = context;
	}

	public FieldIdentifier Field { get; }
	public ObjectGraphValidator Validator { get; }
	public GraphValidationContext Context { get; }

	public void Deconstruct(out FieldIdentifier field, out ObjectGraphValidator validator, out GraphValidationContext context)
	{
		field     = Field;
		validator = Validator;
		context   = Context;
	}
}