using System;
using System.Windows.Input;
using CommunityToolkit.Maui.Behaviors;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.DataForm;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors.Validation;

public sealed class DataFormValidationBehavior : BaseBehavior<Button>
{
    public static readonly BindableProperty DefaultCommitModeProperty
        = BindableProperty.Create(nameof(DefaultCommitMode), typeof(DataFormCommitMode),
                                  typeof(DataFormValidationBehavior), DataFormCommitMode.PropertyChanged);

    public DataFormCommitMode DefaultCommitMode
    {
        get { return (DataFormCommitMode)GetValue(DefaultCommitModeProperty); }
        set { SetValue(DefaultCommitModeProperty, value); }
    }

    protected override void OnAttachedTo(Button bindable)
    {
        Subject.ValidationMode         =  DataFormValidationMode.Manual;
        Subject.CommitMode             =  DefaultCommitMode;
        BindingContext                 =  bindable.BindingContext;
        bindable.BindingContextChanged += Bindable_BindingContextChanged;
        bindable.Clicked               += BindableOnClicked;

        base.OnAttachedTo(bindable);
    }

    protected override void OnDetachingFrom(Button bindable)
    {
        bindable.BindingContextChanged -= Bindable_BindingContextChanged;
        bindable.Clicked               -= BindableOnClicked;

        base.OnDetachingFrom(bindable);
    }

    void Bindable_BindingContextChanged(object? sender, EventArgs e)
    {
        if (sender is BindableObject b)
        {
            BindingContext = b.BindingContext;
        }
    }

    void BindableOnClicked(object? sender, EventArgs e)
    {
        var validate = Subject.Validate(); 
        if (validate && Command.CanExecute(CommandParameter))
        {
            Command.Execute(CommandParameter);
        }
    }

    public readonly static BindableProperty SubjectProperty
        = BindableProperty.Create(nameof(Subject), typeof(SfDataForm), typeof(DataFormValidationBehavior));

    public SfDataForm Subject
    {
        get { return (SfDataForm)GetValue(SubjectProperty); }
        set { SetValue(SubjectProperty, value); }
    }

    public static readonly BindableProperty CommandProperty
        = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(DataFormValidationBehavior));

    public ICommand Command
    {
        get { return (ICommand)GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }

    public static readonly BindableProperty CommandParameterProperty
        = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(DataFormValidationBehavior));

    public object CommandParameter
    {
        get { return GetValue(CommandParameterProperty); }
        set { SetValue(CommandParameterProperty, value); }
    }
}