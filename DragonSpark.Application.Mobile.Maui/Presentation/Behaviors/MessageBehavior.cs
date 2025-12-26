using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using DragonSpark.Compose;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public sealed class MessageBehavior<T> : BehaviorBase<View> where T : class
{
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(MessageBehavior<T>));

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    protected override void OnAttachedTo(View bindable)
    {
        base.OnAttachedTo(bindable);

        WeakReferenceMessenger.Default.Register<T>(this,
                                                   (_, message) =>
                                                   {
                                                       if (Command.Account()?.CanExecute(message) == true)
                                                       {
                                                           Command.Execute(message);
                                                       }
                                                   });
    }

    protected override void OnDetachingFrom(View bindable)
    {
        WeakReferenceMessenger.Default.Unregister<T>(this);
        base.OnDetachingFrom(bindable);
    }
}