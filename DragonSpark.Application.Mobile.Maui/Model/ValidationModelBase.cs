using CommunityToolkit.Mvvm.ComponentModel;

namespace DragonSpark.Application.Mobile.Maui.Model;

public partial class ValidationModelBase : ModelBase, IValidationAware
{
    public ValidationModelBase() : this([]) {}

    public ValidationModelBase(ValidationResults results) => Results = results;

    [ObservableProperty]
    public partial bool IsValid { get; protected set; }

    protected bool Allow() => IsValid && Results.Count == 0;

    [ObservableProperty]
    public partial ValidationResults Results { get; private set; }

    public ValidationResults Get() => Results;
}