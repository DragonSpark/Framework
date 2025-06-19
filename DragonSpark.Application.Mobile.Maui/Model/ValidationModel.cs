using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using DragonSpark.Application.Mobile.Model.Validation;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Maui.Model;

public sealed class ValidationContainer : ObservableObject, IValidationAware, ICondition
{
    public ValidationContainer(bool isValid = true) : this(new ValidationModel(isValid)) {}

    public ValidationContainer(ValidationModel model) => Model = model;

    public ValidationModel Model
    {
        get;
        set
        {
            if (!Equals(value, field))
            {
                field = value;
                OnPropertyChanged();
            }
        }
    }

    public void Execute(ValidationModelRecord parameter)
    {
        var (local, external) = parameter;
        Model                 = new(local, external);
    }

    public IValidationModel Get() => Model;

    public bool Get(None parameter) => Model.Get(parameter);
}

public partial class ValidationModel : ObservableObject, IValidationModel
{
    public ValidationModel(bool isValid = true) : this([], [], isValid) {}

    public ValidationModel(Dictionary<string, string[]> local, Dictionary<string, string[]> external,
                           bool isValid = true)
    {
        IsValid  = isValid;
        Local    = local;
        External = external;
    }

    [ObservableProperty]
    public partial bool IsValid { get; set; }

    public Dictionary<string, string[]> Local { get; }
    public Dictionary<string, string[]> External { get; }

    public bool Get(None parameter) => IsValid && Local.Count == 0;
}