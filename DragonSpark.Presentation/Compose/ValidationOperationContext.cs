using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.Forms.Validation;

namespace DragonSpark.Presentation.Compose
{
	public class ValidationOperationContext : ValidationCallbackContext
	{
		readonly IOperation<ValidationContext> _validation;

		public ValidationOperationContext(IOperation<ValidationContext> validation) : base(validation)
			=> _validation = validation;

		public ValidationCallbackContext DenoteExceptions()
			=> new ValidationCallbackContext(new ExceptionAwareValidationOperation(_validation));

	}
}