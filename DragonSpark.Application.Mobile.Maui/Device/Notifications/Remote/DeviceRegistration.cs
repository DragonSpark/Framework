using DragonSpark.Compose;
using DragonSpark.Model.Operations.Stop;
using Microsoft.Extensions.Logging;
using Switch = DragonSpark.Model.Results.Switch;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;

sealed class DeviceRegistration : IStopAware
{
    readonly IRegisterDevice             _register;
    readonly ILogger<DeviceRegistration> _log;

    public DeviceRegistration(IRegisterDevice register, ILogger<DeviceRegistration> log)
    {
        _register = register;
        _log      = log;
    }

    public async ValueTask Get(CancellationToken parameter)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromDays(1));
        var       run   = new Switch(true);
        while (!parameter.IsCancellationRequested && run)
        {
            try
            {
                while (await timer.WaitForNextTickAsync(parameter).Off())
                {
                    await _register.Off(parameter);
                }
            }
            catch (OperationCanceledException) {}
            catch (Exception ex)
            {
                _log.LogCritical(ex, "A problem was encountered when refreshing device registration");
                run.Down();
            }
        }
    }
}