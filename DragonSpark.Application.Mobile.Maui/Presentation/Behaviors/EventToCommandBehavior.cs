using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public sealed class EventToCommandBehavior : CommunityToolkit.Maui.Behaviors.EventToCommandBehavior
{
    protected override void OnAttachedTo(VisualElement bindable)
    {
        BindingContext = bindable.BindingContext;
        base.OnAttachedTo(bindable);
    }
}