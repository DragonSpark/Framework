using System;
using DragonSpark.Model.Commands;
using Foundation;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Notifications.Remote;

sealed class DiagnosticAwareProcessNotifications : IProcessNotifications
{
    readonly ILogger<DiagnosticAwareProcessNotifications> _logger;
    readonly ICommand<NSDictionary>                             _previous;

    public DiagnosticAwareProcessNotifications(ILogger<DiagnosticAwareProcessNotifications> logger)
        : this(logger, ProcessNotifications.Default) {}

    public DiagnosticAwareProcessNotifications(ILogger<DiagnosticAwareProcessNotifications> logger,
                                               ICommand<NSDictionary> previous)
    {
        _logger   = logger;
        _previous = previous;
    }

    public void Execute(NSDictionary parameter)
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