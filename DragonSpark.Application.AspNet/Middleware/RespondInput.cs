using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Middleware;

public readonly record struct RespondInput(HttpContext Context, int Code);