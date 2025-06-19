using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.DataForm;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors.Validation;

public sealed class DataFormValidationBehavior : DataFormValidationBehaviorBase
{
    public static readonly BindableProperty DefaultCommitModeProperty
        = BindableProperty.Create(nameof(DefaultCommitMode), typeof(DataFormCommitMode),
                                  typeof(DataFormValidationBehavior), DataFormCommitMode.PropertyChanged);

    public DataFormCommitMode DefaultCommitMode
    {
        get { return (DataFormCommitMode)GetValue(DefaultCommitModeProperty); }
        set { SetValue(DefaultCommitModeProperty, value); }
    }

    public static readonly BindableProperty DefaultValidationModeProperty
        = BindableProperty.Create(nameof(DefaultValidationMode), typeof(DataFormValidationMode),
                                  typeof(DataFormValidationBehavior), DataFormValidationMode.PropertyChanged);

    public DataFormValidationMode DefaultValidationMode
    {
        get { return (DataFormValidationMode)GetValue(DefaultValidationModeProperty); }
        set { SetValue(DefaultValidationModeProperty, value); }
    }

    protected override void OnAttachedTo(SfDataForm bindable)
    {
        BindingContext            =  bindable.BindingContext;
        bindable.ValidationMode   =  DefaultValidationMode;
        bindable.CommitMode       =  DefaultCommitMode;
        bindable.ValidateProperty += BindableOnValidateProperty;
        base.OnAttachedTo(bindable);
    }

    void BindableOnValidateProperty(object? sender, DataFormValidatePropertyEventArgs e)
    {
        if (e.IsValid)
        {
            Model.Local.Remove(e.PropertyName);
        }
        else
        {
            Model.Local[e.PropertyName] = [e.ErrorMessage];
        }

        Command.NotifyCanExecuteChanged();
    }

    protected override void OnDetachingFrom(SfDataForm bindable)
    {
        bindable.ValidateProperty -= BindableOnValidateProperty;

        base.OnDetachingFrom(bindable);
    }

    public readonly static BindableProperty CommandProperty
        = BindableProperty.Create(nameof(Command), typeof(IRelayCommand), typeof(DataFormValidationBehavior));

    public IRelayCommand Command
    {
        get { return (IRelayCommand)GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }
}