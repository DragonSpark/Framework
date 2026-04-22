using DragonSpark.Application.Mobile.Maui.Messaging;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote.Messages;

sealed class ProcessNotification : Command<ProcessNotificationInput>, IProcessNotification
{
    public ProcessNotification(ComposeNotificationMessage message)
        : this(message, Send<NotificationReceivedMessage>.Default) {}

    public ProcessNotification(ComposeNotificationMessage message, ICommand<NotificationReceivedMessage> send)
        : base(message.Then().Terminate(send)) {}
}