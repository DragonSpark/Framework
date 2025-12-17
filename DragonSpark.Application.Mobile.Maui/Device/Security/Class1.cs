using System;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Maui.Presentation;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Selection.Conditions;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace DragonSpark.Application.Mobile.Maui.Device.Security;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}
    
    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<ISupportsPasskey>()
                 .Forward<SupportsPasskey>()
                 .Decorate<SimulatorAwareSupportsPasskey>()
                 .Singleton();
    }
}

public interface ISupportsPasskey : IDepending;

sealed class SimulatorAwareSupportsPasskey : ISupportsPasskey
{
    readonly IIsSimulator     _simulator;
    readonly ISupportsPasskey _previous;

    public SimulatorAwareSupportsPasskey(IIsSimulator simulator, ISupportsPasskey previous)
    {
        _simulator = simulator;
        _previous  = previous;
    }

    public ValueTask<bool> Get(None parameter) => _simulator.Get() ? false.ToOperation() : _previous.Get(parameter);
}

sealed class PasskeyBrowserDetectionScript : Text.Text
{
    public static PasskeyBrowserDetectionScript Default { get; } = new();

    PasskeyBrowserDetectionScript()
        : base("""
               (async () => {
                   if (!('credentials' in navigator)) return false;
                   if (typeof navigator.credentials.create !== 'function') return false;
                   if (typeof navigator.credentials.get !== 'function') return false;

                   const isUVPAA = await PublicKeyCredential
                       .isUserVerifyingPlatformAuthenticatorAvailable();

                   // Conditional mediation is optional but nice for auto-fill
                   const isCMA = await PublicKeyCredential
                       .isConditionalMediationAvailable?.() ?? false;

                   return isUVPAA && isCMA;
               })();
               """) {}
}

public readonly record struct DeterminePasskeySupportViaBrowserInput(
    WebView Subject,
    TaskCompletionSource<bool> Source);

sealed class DeterminePasskeySupportViaBrowser : IAllocated<DeterminePasskeySupportViaBrowserInput>
{
    public static DeterminePasskeySupportViaBrowser Default { get; } = new();

    DeterminePasskeySupportViaBrowser() : this(PopModal.Default, PasskeyBrowserDetectionScript.Default) {}

    readonly IAllocated<bool> _pop;
    readonly string           _script;

    public DeterminePasskeySupportViaBrowser(IAllocated<bool> pop, string script)
    {
        _pop    = pop;
        _script = script;
    }

    public async Task Get(DeterminePasskeySupportViaBrowserInput parameter)
    {
        var (subject, source) = parameter;
        try
        {
            var evaluate = await subject.EvaluateJavaScriptAsync(_script).Off();
            source.SetResult(evaluate.Trim().ToLower() == "true");
        }
        catch
        {
            source.SetResult(false);
        }
        finally
        {
            await _pop.Main(false).Off();
        }
    }
}

sealed class PasskeyHandler : ISelect<DeterminePasskeySupportViaBrowserInput, EventHandler<WebNavigatedEventArgs>>
{
    public static PasskeyHandler Default { get; } = new();

    PasskeyHandler() : this(DeterminePasskeySupportViaBrowser.Default) {}

    readonly IAllocated<DeterminePasskeySupportViaBrowserInput> _support;

    public PasskeyHandler(IAllocated<DeterminePasskeySupportViaBrowserInput> support)
    {
        _support = support;
    }

    public EventHandler<WebNavigatedEventArgs> Get(DeterminePasskeySupportViaBrowserInput parameter)
    {
        var (content, source) = parameter;
        return (_, _) => _support.Off(new(content, source));
    }
}

sealed class SupportsPasskey : ISupportsPasskey
{
    public static SupportsPasskey Default { get; } = new();

    SupportsPasskey() : this(PasskeyHandler.Default, PushModal.Default) {}

    readonly ISelect<DeterminePasskeySupportViaBrowserInput, EventHandler<WebNavigatedEventArgs>> _support;
    readonly IAllocated<PushModalInput>                                                           _push;

    public SupportsPasskey(ISelect<DeterminePasskeySupportViaBrowserInput, EventHandler<WebNavigatedEventArgs>> support,
                           IAllocated<PushModalInput> push)
    {
        _support = support;
        _push    = push;
    }

    public async ValueTask<bool> Get(None parameter)
    {
        var content = new WebView { HeightRequest       = 1, WidthRequest             = 1, IsVisible = false };
        var page    = new ContentPage { BackgroundColor = Colors.Transparent, Content = content };
        var source  = new TaskCompletionSource<bool>();
        var handler = _support.Get(new(content, source));

        content.Navigated += handler;

        content.Source = "about:blank";

        var result = await source.Task.Off();

        await _push.Off(new(page, false));
        content.Navigated -= handler;

        return result;
    }
}