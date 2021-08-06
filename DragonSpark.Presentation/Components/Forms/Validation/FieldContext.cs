using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public readonly struct FieldContext
	{
		public FieldContext(EditContext context, FieldIdentifier identifier)
		{
			Context    = context;
			Identifier = identifier;
		}

		public EditContext Context { get; }

		public FieldIdentifier Identifier { get; }

		public void Deconstruct(out EditContext context, out FieldIdentifier field)
		{
			context = Context;
			field   = Identifier;
		}
	}
}