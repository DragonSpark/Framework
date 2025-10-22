using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.DataForm;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public sealed class DataFormItemBehavior : BehaviorBase<SfDataForm>
{
    public readonly static BindableProperty PaddingProperty
        = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(DataFormItemBehavior),
                                  new Thickness(0, 0, 0, 0));

    public readonly static BindableProperty IsReadOnlyProperty
        = BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(DataFormItemBehavior), false);

    public bool IsReadOnly
    {
        get { return (bool)GetValue(IsReadOnlyProperty); }
        set { SetValue(IsReadOnlyProperty, value); }
    }

    public Thickness Padding
    {
        get { return (Thickness)GetValue(PaddingProperty); }
        set { SetValue(PaddingProperty, value); }
    }

    protected override void OnAttachedTo(SfDataForm bindable)
    {
        bindable.GenerateDataFormItem += BindableOnGenerateDataFormItem;
        base.OnAttachedTo(bindable);
    }

    protected override void OnDetachingFrom(SfDataForm bindable)
    {
        bindable.GenerateDataFormItem -= BindableOnGenerateDataFormItem;
        base.OnDetachingFrom(bindable);
    }

    void BindableOnGenerateDataFormItem(object? sender, GenerateDataFormItemEventArgs e)
    {
        var item = e.DataFormItem;
        item.Padding    = Padding;
        item.IsReadOnly = IsReadOnly;
    }
}