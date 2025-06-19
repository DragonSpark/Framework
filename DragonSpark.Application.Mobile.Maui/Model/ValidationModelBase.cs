using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Maui.Model;

public partial class ValidationModelBase : ModelBase
{
    protected ValidationModelBase(bool isValid = true) : this(new ValidationModel(isValid)) {}

    protected ValidationModelBase(ValidationModel validation) => Validation = validation;

    [ObservableProperty]
    public partial ValidationModel Validation { get; set; }
}

// TODO

public partial class ValidationModel : ObservableObject, ICondition
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