using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Presentation
{
	public interface IAuthenticateAction : IOperationResult<CallbackContext, IActionResult> {}
}