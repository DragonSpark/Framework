using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Application.Hosting.AzureFunctions
{
	public interface IApplicationContexts
		: IApplicationContexts<AzureFunctionParameter, IApplicationContext<AzureFunctionParameter, IActionResult>> {}
}