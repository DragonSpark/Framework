using DragonSpark.Application.Mobile.Maui.Model;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.DataForm;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors.Validation;

public class DataFormValidationBehaviorBase : BehaviorBase<SfDataForm>
{
    public readonly static BindableProperty ModelProperty
        = BindableProperty.Create(nameof(Model), typeof(ValidationModel),
                                  typeof(DataFormValidationBehaviorBase),
                                  propertyChanged:
                                  (bindable, _, newValue) =>
                                  {
                                      if (bindable is DataFormValidationBehaviorBase b && newValue is ValidationModel m)
                                      {
                                          b.OnSubjectChanged(m);
                                      }
                                  });

    protected virtual void OnSubjectChanged(ValidationModel parameter) {}

    public ValidationModel Model
    {
        get { return (ValidationModel)GetValue(ModelProperty); }
        set { SetValue(ModelProperty, value); }
    }
}