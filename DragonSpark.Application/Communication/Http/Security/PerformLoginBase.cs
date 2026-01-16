using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Communication.Http.Security;

public class PerformLoginBase : IPerformLogin
{
    readonly IPerformLogin  _previous;
    readonly ICompleteLogin _complete;

    protected PerformLoginBase(IPerformLogin previous, ICompleteLogin complete)
    {
        _previous = previous;
        _complete = complete;
    }

    public async ValueTask<AccessTokenResponse?> Get(Stop<LoginRequest> parameter)
    {
        var ((address, _), stop) = parameter;
        var result = await _previous.Off(parameter);

        await _complete.Off(new(result is not null ? new(address, result) : null, stop));

        return result;
    }
}