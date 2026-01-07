using System.Threading.Tasks;
using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Mobile.Maui.Device.Security;

public class StatusAwareLogin : ILogin
{
    readonly ILogin                     _previous;
    readonly IStorageValue<LoginStatus> _save;

    protected StatusAwareLogin(ILogin previous) : this(previous, LoginStatusStorageValue.Default) {}

    protected StatusAwareLogin(ILogin previous, IStorageValue<LoginStatus> save)
    {
        _previous = previous;
        _save     = save;
    }

    public async ValueTask<AccessTokenResponse?> Get(Stop<LoginRequest> parameter)
    {
        var ((address, _), stop) = parameter;
        var result = await _previous.Off(parameter);
        if (result is not null)
        {
            await _save.Off(new LoginStatus(address, false).Stop(stop));
        }
        else
        {
            await _save.Get(None.Default.Stop(stop)).Off();
        }

        return result;
    }
}