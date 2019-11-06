using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Application.Hosting.AzureFunctions
{
	public sealed class AzureFunction<T> : IAzureFunction where T : class, IAzureFunction
	{
		public static AzureFunction<T> Default { get; } = new AzureFunction<T>();

		AzureFunction() : this(ApplicationContexts<T>.Default) {}

		readonly IApplicationContexts _contexts;

		public AzureFunction(IApplicationContexts contexts) => _contexts = contexts;

		public IActionResult Get(AzureFunctionParameter parameter)
		{
			using (var context = _contexts.Get(parameter))
			{
				return context.Get(parameter);
			}
		}
	}
}