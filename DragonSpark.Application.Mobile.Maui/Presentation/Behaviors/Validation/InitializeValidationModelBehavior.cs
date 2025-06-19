using Microsoft.Maui.Controls;
using Syncfusion.Maui.DataForm;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors.Validation;

public sealed class InitializeValidationModelBehavior : DataFormValidationBehaviorBase
{
    public static readonly BindableProperty InitialIsValidProperty
        = BindableProperty.Create(nameof(InitialIsValid), typeof(bool), typeof(InitializeValidationModelBehavior),
                                  true);

    public bool InitialIsValid
    {
        get { return (bool)GetValue(InitialIsValidProperty); }
        set { SetValue(InitialIsValidProperty, value); }
    }

    protected override void OnAttachedTo(SfDataForm bindable)
    {
        base.OnAttachedTo(bindable);
        bindable.ValidateProperty += BindableOnValidateProperty;
    }

    void BindableOnValidateProperty(object? sender, DataFormValidatePropertyEventArgs e)
    {
        Model.IsValid = InitialIsValid;
        if (View is not null)
        {
            View.ValidateProperty -= BindableOnValidateProperty;
        }
    }

    protected override void OnDetachingFrom(SfDataForm bindable)
    {
        bindable.ValidateProperty -= BindableOnValidateProperty;
        base.OnDetachingFrom(bindable);
    }
}