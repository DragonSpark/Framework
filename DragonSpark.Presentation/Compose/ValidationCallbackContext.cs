using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;

namespace DragonSpark.Presentation.Compose
{
	public class ValidationCallbackContext : IResult<CallbackContext<Components.Forms.ValidationContext>>
	{
		readonly IOperation<Components.Forms.ValidationContext> _validation;

		public ValidationCallbackContext(IOperation<Components.Forms.ValidationContext> validation)
			=> _validation = validation;

		public CallbackContext<Components.Forms.ValidationContext> Get()
			=> new CallbackContext<Components.Forms.ValidationContext>(_validation.Then().Demote());
	}
}