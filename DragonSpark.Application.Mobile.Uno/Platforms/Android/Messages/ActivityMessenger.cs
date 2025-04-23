using System.Threading.Tasks;
using Android.Content;
using DragonSpark.Application.Mobile.Uno.Device.Messaging;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations.Allocated;
using Microsoft.Extensions.Logging;
using Uno.UI;
using Exception = System.Exception;

namespace DragonSpark.Application.Mobile.Uno.Platforms.Android.Messages;

sealed class ActivityMessenger : IMessenger
{
    readonly Error        _error;
    readonly BaseActivity _activity;

    public ActivityMessenger(Error error) : this(error, BaseActivity.Current) {}

    [Candidate(false)]
    public ActivityMessenger(Error error, BaseActivity activity)
    {
        _error    = error;
        _activity = activity;
    }

    public ValueTask<bool> Get(Token<MessageInput> parameter)
    {
        var ((recipient, message), _) = parameter;
        try
        {
            var intent = new Intent(Intent.ActionView, global::Android.Net.Uri.Parse($"sms:{recipient}"));
            intent.PutExtra("sms_body", message);
            _activity.StartActivity(intent);

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
        public Error(ILogger<Error> logger) : base(logger, "Failed to launch SMS application for {Number}") {}
    }
}