using CommunityToolkit.Mvvm.ComponentModel;
using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Maui.Model;

public partial class ValidationModelBase : ModelBase, IValidationAware
{
    protected ValidationModelBase() : this(new()) {}

    protected ValidationModelBase(ValidationResults results) => Results = results;

    [ObservableProperty]
    public partial bool IsValid { get; protected set; }

    protected bool Allow() => IsValid && Results.Get();

    [ObservableProperty]
    public partial ValidationResults Results { get; private set; }

    public ValidationResults Get() => Results;
}