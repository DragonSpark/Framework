using System;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Maui.Presentation;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace DragonSpark.Application.Mobile.Maui.Device.Security;

public sealed class SupportsPasskey : ISupportsPasskey
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

        await _push.Off(new(page, false));

        content.Navigated += handler;
        content.Source    =  "about:blank";

        var result = await source.Task.Off();

        content.Navigated -= handler;

        return result;
    }
}