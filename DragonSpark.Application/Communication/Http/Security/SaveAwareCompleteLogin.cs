using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Communication.Http.Security;

public class SaveAwareCompleteLogin : ICompleteLogin
{
    readonly ICompleteLogin        _previous;
    readonly IUpdateTokenState _save;

    protected SaveAwareCompleteLogin(ICompleteLogin previous, IUpdateTokenState save)
    {
        _previous = previous;
        _save     = save;
    }

    public async ValueTask<AccessTokenResponse?> Get(Stop<LoginRequest> parameter)
    {
        var ((address, _), stop) = parameter;
        var result  = await _previous.Off(parameter);
        var subject = result is not null ? new AccessTokenView(address, result) : null;
        await _save.Off(new(subject, stop));
        return result;
    }
}