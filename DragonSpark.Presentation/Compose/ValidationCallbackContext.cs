using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Components.Forms;

namespace DragonSpark.Presentation.Compose
{
	public class ValidationCallbackContext : IResult<CallbackContext<Validation>>
	{
		readonly IOperation<Validation> _validation;

		public ValidationCallbackContext(IOperation<Validation> validation) => _validation = validation;

		public CallbackContext<Validation> Get() => new CallbackContext<Validation>(_validation.Then().Demote());
	}
}