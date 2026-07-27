using DragonSpark.Compose;
using DragonSpark.Model.Operations.Stop;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Server.Mobile.Notifications;

sealed class PurgeExpiredRegistrations : IStopAware
{
    readonly NotificationHubClient              _client;
    readonly ILogger<PurgeExpiredRegistrations> _logger;
    readonly byte                               _size;

    public PurgeExpiredRegistrations(NotificationHubClients clients, ILogger<PurgeExpiredRegistrations> logger,
                                     CleanUpSettings settings)
        : this(clients.Server, logger, settings.BatchSize) {}

    public PurgeExpiredRegistrations(NotificationHubClient client, ILogger<PurgeExpiredRegistrations> logger, byte size)
    {
        _client = client;
        _logger = logger;
        _size   = size;
    }

    public async ValueTask Get(CancellationToken parameter)
    {
        try
        {
            var count = 0;
            var token = string.Empty;

            do
            {
                var registrations = await _client.GetAllRegistrationsAsync(token, _size, parameter).Off();

                token = registrations.ContinuationToken;

                foreach (var registration in registrations)
                {
                    if (registration.ExpirationTime < DateTime.UtcNow)
                    {
                        var installation = RegistrationInstallation.Default.Get(registration);
                        var task = installation is not null
                                       ? _client.DeleteInstallationAsync(installation, parameter)
                                       : _client.DeleteRegistrationAsync(registration, parameter);

                        await task.Off();

                        count++;
                    }
                }
            } while (!token.IsNullOrEmpty());

            _logger.LogInformation("Cleanup completed. Deleted {Count} expired registrations.", count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during expired registrations cleanup");
        }
    }
}