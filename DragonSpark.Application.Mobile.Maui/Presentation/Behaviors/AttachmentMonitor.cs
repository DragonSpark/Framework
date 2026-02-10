using System;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

sealed class AttachmentMonitor : ICommand, IDisposable
{
    readonly Behavior                _host;
    readonly VisualElement           _subject;
    readonly AttachmentMonitorEvents _events;

    // ReSharper disable once TooManyDependencies
    public AttachmentMonitor(Behavior host, VisualElement subject) : this(host, subject, _ => {}) {}

    public AttachmentMonitor(Behavior host, VisualElement subject, Action<VisualElement> detaching)
        : this(host, subject, new AttachmentMonitorEvents(detaching)) {}

    public AttachmentMonitor(Behavior host, VisualElement subject, AttachmentMonitorEvents events)
    {
        _host    = host;
        _subject = subject;
        _events  = events;
    }

    public void Execute(None parameter)
    {
        _subject.Unloaded              += OnUnloaded;
        _subject.BindingContextChanged += Changed;
        _host.BindingContext           =  _subject.BindingContext;
    }

    void OnUnloaded(object? sender, EventArgs e)
    {
        if (sender is VisualElement view)
        {
            _events.Detaching(view);
        }
    }

    void Changed(object? sender, EventArgs e)
    {
        if (sender is BindableObject b)
        {
            _host.BindingContext = b.BindingContext;
            _events.Changed(b);
        }
    }

    public void Dispose()
    {
        _subject.Unloaded              -= OnUnloaded;
        _subject.BindingContextChanged -= Changed;
    }
}