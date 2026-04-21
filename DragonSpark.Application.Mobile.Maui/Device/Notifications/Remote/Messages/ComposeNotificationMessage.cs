using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote.Messages;

sealed class ComposeNotificationMessage : ISelect<ProcessNotificationInput, NotificationReceivedMessage>
{
    readonly IActionParser _parser;

    public ComposeNotificationMessage(IActionParser parser) => _parser = parser;

    public NotificationReceivedMessage Get(ProcessNotificationInput parameter)
    {
        var (title, body, action) = parameter;

        if (!action.IsNullOrEmpty())
        {
            var command = _parser.Get(action);
            if (command is not null)
            {
                return new ActionReceivedMessage(title, body, command);
            }
        }

        return new AlertReceivedMessage(title, body);
    }
}