using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using Metalama.Extensions.DependencyInjection;
using Metalama.Framework.Aspects;
using Microsoft.Extensions.DependencyInjection;
using Syncfusion.Maui.Toolkit.Popup;

namespace DragonSpark.Application.Mobile.Maui.Aspects;

public sealed class ConfirmUsingAttribute : OverrideMethodAspect
{
    [IntroduceDependency(IsRequired = true)]
    readonly IServiceProvider? _services = null;
    readonly Type _type;

    public ConfirmUsingAttribute(Type type)
    {
        _type                           = type;
        UseAsyncTemplateForAnyAwaitable = true;
    }

    public override async Task<dynamic?> OverrideAsyncMethod()
    {
        var popup  = _services?.GetRequiredService(_type) is SfPopup p ? p : null;
        if (popup != null)
        {
            popup.BindingContext = meta.This;
            if (!await popup.ShowAsync().On())
            {
                return null;
            }
        }

        return await meta.ProceedAsync().Off();
    }

    public override dynamic? OverrideMethod() => meta.Proceed();
}