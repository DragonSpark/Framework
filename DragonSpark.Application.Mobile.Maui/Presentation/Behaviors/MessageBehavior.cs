using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using DragonSpark.Compose;
using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Behaviors;

public sealed class MessageBehavior<T> : BehaviorBase, IRecipient<T> where T : class
{
    readonly IMessenger _messenger;

    public MessageBehavior() : this(WeakReferenceMessenger.Default) {}

    public MessageBehavior(IMessenger messenger) => _messenger = messenger;

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(MessageBehavior<T>));

    protected override void OnAttached(VisualElement bindable)
    {
        base.OnAttached(bindable);
        _messenger.Register(this);        
    }

    public void Receive(T message)
    {
        if (Command.Account()?.CanExecute(message) == true)
        {
            Command.Execute(message);
        }
    }

    protected override void OnDetached(VisualElement bindable)
    {
        _messenger.Unregister<T>(this);
        base.OnDetached(bindable);
    }
}