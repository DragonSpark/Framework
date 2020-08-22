using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.Forms;

namespace DragonSpark.Presentation.Compose
{
	public class ValidationOperationContext : ValidationCallbackContext
	{
		readonly IOperation<Validation> _validation;

		public ValidationOperationContext(IOperation<Validation> validation) : base(validation)
			=> _validation = validation;

		public ValidationCallbackContext DenoteExceptions()
			=> new ValidationCallbackContext(new ExceptionAwareValidationOperation(_validation));

	}
}