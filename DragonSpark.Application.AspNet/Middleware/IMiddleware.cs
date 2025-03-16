using DragonSpark.Model.Operations.Allocated;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Middleware;

public interface IMiddleware : IAllocated<MiddlewareInput>
{
	Task Get(HttpContext context, RequestDelegate next);
}