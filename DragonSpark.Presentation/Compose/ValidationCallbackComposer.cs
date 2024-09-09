using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components.Forms.Validation;

namespace DragonSpark.Presentation.Compose;

public class ValidationCallbackComposer : IResult<CallbackComposer<ValidationContext>>
{
	readonly IOperation<ValidationContext> _validation;

	public ValidationCallbackComposer(IOperation<ValidationContext> validation) => _validation = validation;

	public CallbackComposer<ValidationContext> Get() => new(_validation.Then().Allocate());
}