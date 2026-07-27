using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;

sealed class RegisterDevice : IRegisterDevice
{
    readonly IRegisterDeviceToken                                          _register;
    readonly DragonSpark.Model.Operations.Results.Stop.IStopAware<string?> _token;

    public RegisterDevice(IRegisterDeviceToken register) : this(register, DeviceToken.Default) {}

    public RegisterDevice(IRegisterDeviceToken register,
                          DragonSpark.Model.Operations.Results.Stop.IStopAware<string?> token)
    {
        _register = register;
        _token    = token;
    }

    public async ValueTask Get(CancellationToken parameter)
    {
        var token = await _token.Off(parameter);
        if (token is not null)
        {
            await _register.Off(new(token, parameter));
        }
    }
}