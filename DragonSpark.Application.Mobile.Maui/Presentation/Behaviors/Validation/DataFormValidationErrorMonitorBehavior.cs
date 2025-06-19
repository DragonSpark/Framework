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

    protected override void OnSubjectChanged(ValidationModel parameter)
    {
        if (parameter.Get())
        {
            var keys  = parameter.External.Keys;
            if (keys.Count > 0)
            {
                View?.Validate(keys.ToList());
            }
        }
    }

    void BindableOnValidateProperty(object? sender, DataFormValidatePropertyEventArgs e)
    {
        if (e.IsValid)
        {
            var valid = !Model.External.ContainsKey(e.PropertyName);
            e.IsValid = valid;
            e.ErrorMessage = valid.Inverse() && Model.External.TryGetValue(e.PropertyName, out var messages)
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