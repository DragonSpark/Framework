using System;
using System.Threading.Tasks;
using DragonSpark.Application.Model;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Runtime;
using Microsoft.Azure.NotificationHubs;

namespace DragonSpark.Server.Mobile.Notifications;

sealed class DeviceRegistration : IDeviceRegistration
{
    readonly NotificationHubClient _client;
    readonly TimeSpan              _expiry;
    readonly ITime                 _time;

    public DeviceRegistration(NotificationHubClients clients, NotificationHubSettings settings)
        : this(clients.Client, settings.TimeToLive, Time.Default) {}

    public DeviceRegistration(NotificationHubClient client, TimeSpan expiry, ITime time)
    {
        _client = client;
        _expiry = expiry;
        _time   = time;
    }

    public ValueTask Get(Stop<UserInput<DeviceRegistrationInput>> parameter)
    {
        var ((user, (installationId, deviceToken, platform)), stop) = parameter;

        var installation = new Installation
        {
            InstallationId = installationId,
            Platform       = platform,
            PushChannel    = deviceToken,
            ExpirationTime = _time.Get().UtcDateTime + _expiry,
            UserId         = user.ToString(),
            Tags           = [$"identity:{Guid.NewGuid()}"]
        };

        return _client.CreateOrUpdateInstallationAsync(installation, stop).ToOperation();
    }
}