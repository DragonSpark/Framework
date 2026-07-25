using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Middleware;

abstract class MiddlewareBase : IMiddleware
{
	public abstract Task Get(MiddlewareInput parameter);

	public Task Get(HttpContext context, RequestDelegate next) => Get(new(context, next));
}