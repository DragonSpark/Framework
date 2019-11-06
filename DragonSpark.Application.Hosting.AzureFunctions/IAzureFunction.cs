using Microsoft.AspNetCore.Mvc;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Hosting.AzureFunctions
{
	public interface IAzureFunction : ISelect<AzureFunctionParameter, IActionResult> {}
}