using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Middleware;

abstract class MiddlewareBase : IMiddleware
{
	public abstract Task Get(MiddlewareInput parameter);

	public Task Get(HttpContext context, RequestDelegate next) => Get(new(context, next));
}