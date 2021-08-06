using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public readonly struct ValidationContext
	{
		public ValidationContext(FieldContext field, ValidationMessageStore messages, FieldValidationMessages messaging)
		{
			Field     = field;
			Messages  = messages;
			Messaging = messaging;
		}

		public FieldContext Field { get; }

		public ValidationMessageStore Messages { get; }
		public FieldValidationMessages Messaging { get; }

		public void Deconstruct(out FieldContext context, out ValidationMessageStore messages,
		                        out FieldValidationMessages messaging)
		{
			context   = Field;
			messages  = Messages;
			messaging = Messaging;
		}
	}
}