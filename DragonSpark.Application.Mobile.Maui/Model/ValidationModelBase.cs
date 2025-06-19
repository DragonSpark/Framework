namespace DragonSpark.Application.Mobile.Maui.Model;

public class ValidationModelBase : ModelBase
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
}