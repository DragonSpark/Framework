using Android.Gms.Tasks;
using DragonSpark.Application.Mobile.Maui.Presentation;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Conditions;
using Firebase.Messaging;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications.Remote;

sealed class InitializeToken : ICommand
{
    public static InitializeToken Default { get; } = new();

    InitializeToken() : this(IsSupported.Default, FirebaseMessaging.Instance, TokenReceived.Default) {}

    readonly ICondition         _supported;
    readonly FirebaseMessaging  _messaging;
    readonly IOnSuccessListener _success;

    public InitializeToken(ICondition supported, FirebaseMessaging messaging, IOnSuccessListener success)
    {
        _supported = supported;
        _messaging = messaging;
        _success   = success;
    }

    public void Execute(None parameter)
    {
        var logger = CurrentService<ILogger<InitializeToken>>.Default.Get();
        var supported      = _supported.Get();
        logger.LogInformation("SUPPORTED! - {Supported}", supported);
        if (supported)
        {
            _messaging.GetToken().AddOnSuccessListener(_success);
        }
    }
}