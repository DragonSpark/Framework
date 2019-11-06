using Microsoft.AspNetCore.Mvc;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Hosting.AzureFunctions
{
	sealed class ApplicationContexts<T> :
		ApplicationContexts<AzureFunctionContext<T>, AzureFunctionParameter, IActionResult>,
		IApplicationContexts where T : class, ISelect<AzureFunctionParameter, IActionResult>
	{
		public static ApplicationContexts<T> Default { get; } = new ApplicationContexts<T>();

		ApplicationContexts() : base(Start.A.Selection<AzureFunctionParameter>().By.Default<IServices>()) {}
	}
}