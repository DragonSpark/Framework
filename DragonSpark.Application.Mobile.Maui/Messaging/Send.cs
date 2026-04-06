using CommunityToolkit.Mvvm.Messaging;
using DragonSpark.Application.Mobile.Maui.Runtime;

namespace DragonSpark.Application.Mobile.Maui.Messaging;

public sealed class Send<T> : MainThreadAwareCommand<T> where T : class
{
    public static Send<T> Default { get; } = new();

    Send() : this(WeakReferenceMessenger.Default) {}

    public Send(IMessenger messenger) : base(x => messenger.Send(x)) {}
}
