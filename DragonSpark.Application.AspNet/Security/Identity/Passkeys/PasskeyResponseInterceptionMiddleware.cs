using System.Threading.Tasks;
using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

sealed class PasskeyResponseInterceptionMiddleware
{
    readonly ShouldIntercept       _should;
    readonly InterceptLoginRequest _intercept;
    readonly RequestDelegate       _next;

    public PasskeyResponseInterceptionMiddleware(ShouldIntercept should, RequestDelegate next,
                                                 InterceptLoginRequest intercept)
    {
        _next      = next;
        _should    = should;
        _intercept = intercept;
    }

    [UsedImplicitly]
    public Task InvokeAsync(HttpContext context)
        => _should.Get(context.Request)
               ? _intercept.Allocate(new(new(_next, context), context.RequestAborted))
               : _next(context);
}