using Android.Content;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Notifications.Remote;

sealed class Initialize : IInitialize
{
    readonly ICommand            _token;
    readonly ICommand<Intent>    _process;
    readonly ILogger<Initialize> _logger;

    public Initialize(IProcessNotifications notifications, ILogger<Initialize> logger)
        : this(InitializeToken.Default, notifications, logger) {}

    public Initialize(ICommand token, ICommand<Intent> process, ILogger<Initialize> logger)
    {
        _token   = token;
        _process = process;
        _logger  = logger;
        _logger.LogInformation("Initialized CONSTRUCTOR!");
    }

    public void Execute(Intent parameter)
    {
        _logger.LogInformation("Initialized INTENT!");
        _token.Execute();
        _process.Execute(parameter);
    }
}