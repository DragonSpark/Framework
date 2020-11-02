using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Application.Components.Validation
{
	public readonly struct NewValidationContext
	{
		public NewValidationContext(FieldIdentifier field, ObjectGraphValidator validator)
			: this(field, validator, new ModelValidationContext()) {}

		public NewValidationContext(object instance, ObjectGraphValidator validator, ModelValidationContext context)
			: this(new FieldIdentifier(instance, string.Empty), validator, context) {}

		public NewValidationContext(FieldIdentifier field, ObjectGraphValidator validator, ModelValidationContext context)
		{
			Field  = field;
			Validator = validator;
			Context   = context;
		}

		public FieldIdentifier Field { get; }
		public ObjectGraphValidator Validator { get; }
		public ModelValidationContext Context { get; }

		public void Deconstruct(out FieldIdentifier field, out ObjectGraphValidator validator, out ModelValidationContext context)
		{
			field     = Field;
			validator = Validator;
			context   = Context;
		}
	}
}