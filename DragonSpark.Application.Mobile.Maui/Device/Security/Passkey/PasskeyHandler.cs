using System;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Device.Security.Passkey;

sealed class PasskeyHandler : ISelect<DeterminePasskeySupportViaBrowserInput, EventHandler<WebNavigatedEventArgs>>
{
    public static PasskeyHandler Default { get; } = new();

    PasskeyHandler() : this(DeterminePasskeySupportViaBrowser.Default) {}

    readonly IAllocated<DeterminePasskeySupportViaBrowserInput> _support;

    public PasskeyHandler(IAllocated<DeterminePasskeySupportViaBrowserInput> support) => _support = support;

    public EventHandler<WebNavigatedEventArgs> Get(DeterminePasskeySupportViaBrowserInput parameter)
    {
        var (content, source) = parameter;
        return (_, _) => _support.Off(new(content, source));
    }
}