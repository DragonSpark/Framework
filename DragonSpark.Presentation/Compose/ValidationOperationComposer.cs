using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.Forms.Validation;

namespace DragonSpark.Presentation.Compose;

public class ValidationOperationComposer : ValidationCallbackComposer
{
	readonly IOperation<ValidationContext> _validation;

	public ValidationOperationComposer(IOperation<ValidationContext> validation) : base(validation)
		=> _validation = validation;

	public ValidationCallbackComposer DenoteExceptions() => new(new ExceptionAwareValidationOperation(_validation));

}