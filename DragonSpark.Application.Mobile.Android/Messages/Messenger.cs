using System.Threading.Tasks;
using Android.Telephony;
using DragonSpark.Application.Mobile.Device.Messaging;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations.Allocated;
using Microsoft.Extensions.Logging;
using Exception = System.Exception;

namespace DragonSpark.Application.Mobile.Android.Messages;

sealed class Messenger : IMessenger
{
    readonly SmsManager _manager;
    readonly Error      _error;

    public Messenger(Error error) : this(SmsManager.Default.Verify(), error) {}

    [Candidate(false)]
    public Messenger(SmsManager manager, Error error)
    {
        _manager = manager;
        _error   = error;
    }

    public ValueTask<bool> Get(Token<MessageInput> parameter)
    {
        var ((recipient, message), _) = parameter;
        try
        {
            _manager.SendTextMessage(recipient, null, message, null, null);
            return new(true);
        }
        catch (Exception ex)
        {
            _error.Execute(ex, recipient);
            return new(false);
        }
    }

    public sealed class Error : LogErrorException<string>
    {
        public Error(ILogger<Error> logger) : base(logger, "Failed to send SMS to {Number}") {}
    }
}