using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Model;
using DragonSpark.Compose;
using Metalama.Framework.Aspects;

namespace DragonSpark.Application.Mobile.Aspects;

public sealed class ActivityAwareAttribute : OverrideMethodAspect
{
    public ActivityAwareAttribute() => UseAsyncTemplateForAnyAwaitable = true;

    public override async Task<dynamic?> OverrideAsyncMethod()
    {
        var active = meta.This as IActivityAware;
        try
        {
            active?.Execute(true);
            return await meta.ProceedAsync().On();
        }
        finally
        {
            active?.Execute(false);
        }
    }

    public override dynamic? OverrideMethod() => meta.Proceed();
}