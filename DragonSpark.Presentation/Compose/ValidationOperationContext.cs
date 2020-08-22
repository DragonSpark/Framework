using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.Forms;

namespace DragonSpark.Presentation.Compose
{
	public class ValidationOperationContext : ValidationCallbackContext
	{
		readonly IOperation<Components.Forms.ValidationContext> _validation;

		public ValidationOperationContext(IOperation<Components.Forms.ValidationContext> validation) : base(validation)
			=> _validation = validation;

		public ValidationCallbackContext DenoteExceptions()
			=> new ValidationCallbackContext(new ExceptionAwareValidationOperation(_validation));

	}
}