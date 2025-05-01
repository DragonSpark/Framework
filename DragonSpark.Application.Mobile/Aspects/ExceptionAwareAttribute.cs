using System;
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

    public override async Task<dynamic?> OverrideAsyncMethod()
    {
        try
        {
            return await meta.ProceedAsync().Off();
        }
        catch (Exception e) when (_exceptionHandler?.Get(e) ?? false)
        {
            _exceptionHandler.Execute(e);

            return default;
        }
    }

    public override dynamic? OverrideMethod()
    {
        try
        {
            return meta.Proceed();
        }
        catch (Exception e) when (_exceptionHandler?.Get(e) ?? false)
        {
            _exceptionHandler.Execute(e);

            return default;
        }
    }
}