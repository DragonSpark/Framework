using System;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

sealed class AttachmentMonitor : ICommand, IDisposable
{
    readonly Behavior               _host;
    readonly VisualElement          _subject;
    readonly Action<BindableObject> _changed;
    readonly Action<VisualElement>  _detaching;

    // ReSharper disable once TooManyDependencies
    public AttachmentMonitor(Behavior host, VisualElement subject, Action<BindableObject> changed,
                             Action<VisualElement> detaching)
    {
        _host      = host;
        _subject   = subject;
        _changed   = changed;
        _detaching = detaching;
    }

    public void Execute(None parameter)
    {
        _subject.Unloaded              += OnUnloaded;
        _subject.BindingContextChanged += Bindable_BindingContextChanged;
        _host.BindingContext           =  _subject.BindingContext;
    }

    void OnUnloaded(object? sender, EventArgs e)
    {
        if (sender is VisualElement view)
        {
            _host.BindingContext = view.BindingContext;
            _detaching(view);
        }
    }

    void Bindable_BindingContextChanged(object? sender, EventArgs e)
    {
        if (sender is BindableObject b)
        {
            _changed(b);
        }
    }

    public void Dispose()
    {
        _subject.Unloaded              -= OnUnloaded;
        _subject.BindingContextChanged -= Bindable_BindingContextChanged;
    }
}