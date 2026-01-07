using System;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public sealed record AttachmentMonitorEvents(Action<BindableObject> Changed, Action<VisualElement> Detaching)
{
    public AttachmentMonitorEvents(Action<VisualElement> Detaching) : this(_ => {}, Detaching) {}
}