using System.Threading.Tasks;
using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Compose;
using DragonSpark.Contracts.Security;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote;

sealed class LoginAwareDeviceRegistration : ICompleteLogin
{
    readonly ICompleteLogin  _previous;
    readonly IRegisterDevice _register;

    public LoginAwareDeviceRegistration(ICompleteLogin previous, IRegisterDevice register)
    {
        _previous = previous;
        _register = register;
    }

    public async ValueTask Get(Stop<AccessTokenView?> parameter)
    {
        await _previous.Off(parameter);
        await _register.Off(parameter);
    }
}