using System.Threading.Tasks;
using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Mobile.Maui.Security.Identity;

sealed class Login : ILogin
{
    readonly IAccessTokenView                                                     _view;
    readonly DragonSpark.Model.Operations.IStopAware<PersistAccessTokenViewInput> _persist;

    public Login(IAccessTokenView view) : this(view, PersistAccessTokenView.Default) {}

    public Login(IAccessTokenView view, DragonSpark.Model.Operations.IStopAware<PersistAccessTokenViewInput> persist)
    {
        _view    = view;
        _persist = persist;
    }

    public async ValueTask<bool> Get(Stop<ChallengeRequest> parameter)
    {
        var response = await _view.Off(parameter);
        if (response is not null)
        {
            var ((address, _), stop) = parameter;
            await _persist.Off(new(new(address, response), stop));
            return true;
        }

        return false;
    }
}