using DragonSpark.Model.Operations.Stop;
using DragonSpark.Presentation.Components.Forms.Validation;

namespace DragonSpark.Presentation.Compose;

public class ValidationOperationComposer : ValidationCallbackComposer
{
	readonly IStopAware<ValidationContext> _validation;

	public ValidationOperationComposer(IStopAware<ValidationContext> validation) : base(validation)
		=> _validation = validation;

	public ValidationCallbackComposer DenoteExceptions() => new(new ExceptionAwareValidationOperation(_validation));

}