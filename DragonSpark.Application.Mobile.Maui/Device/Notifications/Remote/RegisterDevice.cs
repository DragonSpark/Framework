using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;

sealed class RegisterDevice : IRegisterDevice
{
    readonly IRegisterDeviceToken                                          _register;
    readonly DragonSpark.Model.Operations.Results.Stop.IStopAware<string?> _token;
    readonly ILogger<RegisterDevice>                                       _log;

    public RegisterDevice(IRegisterDeviceToken register, ILogger<RegisterDevice> log)
        : this(register, DeviceToken.Default, log) {}

    public RegisterDevice(IRegisterDeviceToken register,
                          DragonSpark.Model.Operations.Results.Stop.IStopAware<string?> token,
                          ILogger<RegisterDevice> log) // TODO
    {
        _register = register;
        _token    = token;
        _log      = log;
    }

    public async ValueTask Get(CancellationToken parameter)
    {
        var token = await _token.Off(parameter);
        _log.LogInformation("Current device token is {Token}", token);
        if (token is not null)
        {
            await _register.Off(new(token, parameter));
        }
    }
}