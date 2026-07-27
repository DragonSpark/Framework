using Android.Content;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications.Remote;

sealed class DiagnosticAwareProcessNotifications : IProcessNotifications
{
    readonly ILogger<DiagnosticAwareProcessNotifications> _logger;
    readonly ICommand<Intent>                             _previous;

    public DiagnosticAwareProcessNotifications(ILogger<DiagnosticAwareProcessNotifications> logger)
        : this(logger, ReceiveNotification.Default) {}

    public DiagnosticAwareProcessNotifications(ILogger<DiagnosticAwareProcessNotifications> logger,
                                               ICommand<Intent> previous)
    {
        _logger   = logger;
        _previous = previous;
    }

    public void Execute(Intent parameter)
    {
        try
        {
            _previous.Execute(parameter);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception occurred while handling a push notification");
            throw;
        }
    }
}