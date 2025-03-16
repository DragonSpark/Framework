using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Middleware;

/// <summary>
/// Attribution: https://stackoverflow.com/a/74753573
/// </summary>
sealed class OutputCacheSuccessfulRequestsOnlyMiddleware : MiddlewareBase
{
	public static OutputCacheSuccessfulRequestsOnlyMiddleware Default { get; } = new();

	OutputCacheSuccessfulRequestsOnlyMiddleware() {}

	public override Task Get(MiddlewareInput parameter)
	{
		// Do not cache error conditions
		var (context, next) = parameter;
		var response = context.Response;
		response.OnStarting(() =>
		                    {
			                    if (response.StatusCode >= 400)
			                    {
				                    response.Headers.Remove("Cache-Control");
			                    }

			                    return Task.FromResult(0);
		                    });
		return next(context);
	}
}