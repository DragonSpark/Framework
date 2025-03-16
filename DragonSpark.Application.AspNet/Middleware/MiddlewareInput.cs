using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Middleware;

public readonly record struct MiddlewareInput(HttpContext Context, RequestDelegate Next);