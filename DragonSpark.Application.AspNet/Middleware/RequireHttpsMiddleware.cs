using DragonSpark.Application.AspNet.Properties;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Middleware;

sealed class RequireHttpsMiddleware : MiddlewareBase
{
	public static RequireHttpsMiddleware Default { get; } = new();

	RequireHttpsMiddleware() : this(Resources.RequireHttpsMessage) {}

	readonly string _message;

	public RequireHttpsMiddleware(string message) => _message = message;

	public override Task Get(MiddlewareInput parameter)
	{
		var (context, next) = parameter;
		if (!context.Request.IsHttps)
		{
			var response = context.Response;
			response.StatusCode = StatusCodes.Status400BadRequest;
			return response.WriteAsync(_message);
		}

		return next(context);
	}
}