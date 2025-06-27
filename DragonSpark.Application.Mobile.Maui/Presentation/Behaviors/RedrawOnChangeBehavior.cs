using DragonSpark.Compose;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public sealed class RedrawOnChangeBehavior : BehaviorBase<VisualElement>
{
    public readonly static BindableProperty DataObjectProperty
        = BindableProperty.Create(nameof(DataObject), typeof(object), typeof(RedrawOnChangeBehavior), propertyChanged: OnChanged);

    static void OnChanged(BindableObject bindable, object oldValue, object newValue)
    {
        bindable.To<RedrawOnChangeBehavior>().Redraw();
    }

    void Redraw()
    {
        View.Verify().InvalidateMeasure();
    }

    public object DataObject
    {
        get { return GetValue(DataObjectProperty); }
        set { SetValue(DataObjectProperty, value); }
    }
}