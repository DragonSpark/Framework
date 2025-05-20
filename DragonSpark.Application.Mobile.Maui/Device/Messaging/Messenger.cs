using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.ApplicationModel.Communication;

namespace DragonSpark.Application.Mobile.Maui.Device.Messaging;

sealed class Messenger : IMessenger
{
    public static Messenger Default { get; } = new();

    Messenger() {}

    public async ValueTask<bool> Get(Stop<MessageInput> parameter)
    {
        var ((recipient, content), _) = parameter;
        var message = new SmsMessage(content, recipient.Split(',', '*'));
        switch (await Permissions.RequestAsync<Permissions.Sms>().Off())
        {
            case PermissionStatus.Granted:
            case PermissionStatus.Limited:
                await Sms.ComposeAsync(message).Off();
                return true;
        }

        return false;
    }
}