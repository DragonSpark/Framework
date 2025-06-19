using DragonSpark.Application.Mobile.Maui.Model;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.DataForm;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors.Validation;

public class DataFormValidationBehaviorBase : BehaviorBase<SfDataForm>
{
    public readonly static BindableProperty ResultsProperty
        = BindableProperty.Create(nameof(Results), typeof(ValidationResults),
                                  typeof(DataFormValidationBehaviorBase),
                                  propertyChanged:
                                  (bindable, _, newValue)
                                      =>
                                  {
                                      if (bindable is DataFormValidationBehaviorBase b &&
                                          newValue is ValidationResults r)
                                      {
                                          b.OnSubjectChanged(r);
                                      }
                                  });

    protected virtual void OnSubjectChanged(ValidationResults parameter) {}

    public ValidationResults Results
    {
        get { return (ValidationResults)GetValue(ResultsProperty); }
        set { SetValue(ResultsProperty, value); }
    }
}