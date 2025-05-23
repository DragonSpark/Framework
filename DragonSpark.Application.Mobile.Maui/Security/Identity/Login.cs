using System.Threading.Tasks;
using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Mobile.Maui.Security.Identity;

sealed class Login : ILogin
{
    readonly IChallenge _view;

    public Login(IChallenge view) => _view = view;

    public async ValueTask<bool> Get(Stop<ChallengeRequest> parameter)
    {
        var response = await _view.Off(parameter);
        return response is not null;
    }
}