using DragonSpark.Application.Mobile.Model.Validation;

namespace DragonSpark.Application.Mobile.Maui.Model;

public class ValidationModelBase : ModelBase, IValidationAware
{
    protected ValidationModelBase(bool isValid = true) : this(new ValidationModel(isValid)) {}

    // ReSharper disable once VirtualMemberCallInConstructor
    protected ValidationModelBase(ValidationModel validation) => Validation = validation;

    public virtual ValidationModel Validation
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
        Validation            = new(local, external);
    }

    public IValidationModel Get() => Validation;
}