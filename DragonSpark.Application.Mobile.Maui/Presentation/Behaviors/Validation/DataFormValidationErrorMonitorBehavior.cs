using System.Linq;
using DragonSpark.Application.Mobile.Maui.Model;
using DragonSpark.Compose;
using Syncfusion.Maui.DataForm;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors.Validation;

public sealed class DataFormValidationErrorMonitorBehavior : DataFormValidationBehaviorBase
{
    protected override void OnAttachedTo(SfDataForm bindable)
    {
        bindable.ValidateProperty += BindableOnValidateProperty;
        base.OnAttachedTo(bindable);
    }

    protected override void OnSubjectChanged(ValidationResults parameter)
    {
        if (parameter.Get())
        {
            var names = parameter.External.Keys.ToList();
            View?.Validate(names);
        }
    }

    void BindableOnValidateProperty(object? sender, DataFormValidatePropertyEventArgs e)
    {
        if (e.IsValid)
        {
            var valid = !Results.External.ContainsKey(e.PropertyName);
            e.IsValid = valid;
            e.ErrorMessage = valid.Inverse() && Results.External.TryGetValue(e.PropertyName, out var messages)
                                 ? messages[0]
                                 : e.ErrorMessage;   
        }
    }

    protected override void OnDetachingFrom(SfDataForm bindable)
    {
        bindable.ValidateProperty -= BindableOnValidateProperty;
        base.OnDetachingFrom(bindable);
    }
}