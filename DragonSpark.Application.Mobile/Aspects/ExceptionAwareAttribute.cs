using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Diagnostics;
using DragonSpark.Compose;
using Metalama.Extensions.DependencyInjection;
using Metalama.Framework.Aspects;

namespace DragonSpark.Application.Mobile.Aspects;

public sealed class ExceptionAwareAttribute : OverrideMethodAspect
{
    [IntroduceDependency(IsRequired = true)]
    readonly ILastChanceExceptionHandler? _exceptionHandler = null;

    public ExceptionAwareAttribute() => UseAsyncTemplateForAnyAwaitable = true;

    public override async Task<dynamic?> OverrideAsyncMethod()
    {
        try
        {
            return await meta.ProceedAsync().On();
        }
        catch (Exception e) when (_exceptionHandler?.Condition.Get(e) ?? false)
        {
            var parameter = meta.Target.Parameters.LastOrDefault(p => p.Type.Equals(typeof(CancellationToken)));
            var token     = parameter is not null ? parameter.Value : CancellationToken.None;
            await _exceptionHandler.Off(new(e, token));

            return null;
        }
    }

    public override dynamic? OverrideMethod()
    {
        try
        {
            return meta.Proceed();
        }
        catch (Exception e) when (_exceptionHandler?.Condition.Get(e) ?? false)
        {
            _exceptionHandler.Get(new(e)).AsTask().GetAwaiter().GetResult();
            return null;
        }
    }
}