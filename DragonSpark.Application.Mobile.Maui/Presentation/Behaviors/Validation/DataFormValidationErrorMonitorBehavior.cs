using System;
using System.Linq;
using CommunityToolkit.Maui.Behaviors;
using DragonSpark.Application.Mobile.Maui.Model;
using DragonSpark.Compose;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.DataForm;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors.Validation;

public sealed class DataFormValidationErrorMonitorBehavior : BaseBehavior<SfDataForm>
{
    public readonly static BindableProperty ResultsProperty
        = BindableProperty.Create(nameof(Results), typeof(ValidationResults),
                                  typeof(DataFormValidationErrorMonitorBehavior),
                                  propertyChanged:
                                  (bindable, _, newValue)
                                      =>
                                  {
                                      if (bindable is DataFormValidationErrorMonitorBehavior b &&
                                          newValue is ValidationResults r)
                                      {
                                          b.OnSubjectChanged(r);
                                      }
                                  });

    void OnSubjectChanged(ValidationResults parameter)
    {
        if (parameter.Count > 0)
        {
            var names = parameter.Keys.ToList();
            View?.Validate(names);
        }
    }

    public ValidationResults Results
    {
        get { return (ValidationResults)GetValue(ResultsProperty); }
        set { SetValue(ResultsProperty, value); }
    }

    protected override void OnAttachedTo(SfDataForm bindable)
    {
        bindable.BindingContextChanged += Bindable_BindingContextChanged;
        bindable.ValidateProperty      += BindableOnValidateProperty;
        base.OnAttachedTo(bindable);
    }

    void Bindable_BindingContextChanged(object? sender, EventArgs e)
    {
        if (sender is BindableObject b)
        {
            BindingContext = b.BindingContext;
        }
    }

    void BindableOnValidateProperty(object? sender, DataFormValidatePropertyEventArgs e)
    {
        e.IsValid = !Results.ContainsKey(e.PropertyName);
        e.ErrorMessage = e.IsValid.Inverse() && Results.TryGetValue(e.PropertyName, out var messages)
                             ? messages[0]
                             : string.Empty;
    }

    protected override void OnDetachingFrom(SfDataForm bindable)
    {
        bindable.ValidateProperty      -= BindableOnValidateProperty;
        bindable.BindingContextChanged -= Bindable_BindingContextChanged;
        base.OnDetachingFrom(bindable);
    }
}