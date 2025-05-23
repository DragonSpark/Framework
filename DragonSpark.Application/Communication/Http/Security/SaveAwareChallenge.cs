using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Communication.Http.Security;

public class SaveAwareChallenge : IChallenge
{
    readonly IChallenge        _previous;
    readonly IUpdateTokenState _save;

    protected SaveAwareChallenge(IChallenge previous, IUpdateTokenState save)
    {
        _previous = previous;
        _save     = save;
    }

    public async ValueTask<AccessTokenResponse?> Get(Stop<ChallengeRequest> parameter)
    {
        var ((address, _), stop) = parameter;
        var result  = await _previous.Off(parameter);
        var subject = result is not null ? new AccessTokenView(address, result) : null;
        await _save.Off(new(subject, stop));
        return result;
    }
}