using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components.Forms.Validation;

namespace DragonSpark.Presentation.Compose;

public class ValidationCallbackContext : IResult<CallbackContext<ValidationContext>>
{
	readonly IOperation<ValidationContext> _validation;

	public ValidationCallbackContext(IOperation<ValidationContext> validation) => _validation = validation;

	public CallbackContext<ValidationContext> Get() => new(_validation.Then().Allocate());
}