using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components.Forms.Validation;

namespace DragonSpark.Presentation.Compose;

public class ValidationCallbackComposer : IResult<CallbackComposer<Stop<ValidationContext>>>
{
	readonly IStopAware<ValidationContext> _validation;

	public ValidationCallbackComposer(IStopAware<ValidationContext> validation) => _validation = validation;

	public CallbackComposer<Stop<ValidationContext>> Get() => new(_validation.Then().Allocate());
}