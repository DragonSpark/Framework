using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Contracts.Security;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Communication.Http.Security;

public class PerformLoginBase : PerformLoginBase<LoginRequest>, IPerformLogin
{
    protected PerformLoginBase(IStopAware<LoginRequest, AccessTokenResponse?> previous, ICompleteLogin complete)
        : base(previous, complete) {}
}

public class PerformLoginBase<T> : IStopAware<T, AccessTokenResponse?>
    where T : Contracts.Security.LoginRequest
{
    readonly IStopAware<T, AccessTokenResponse?> _previous;
    readonly ICompleteLogin                      _complete;

    protected PerformLoginBase(IStopAware<T, AccessTokenResponse?> previous, ICompleteLogin complete)
    {
        _previous = previous;
        _complete = complete;
    }

    public async ValueTask<AccessTokenResponse?> Get(Stop<T> parameter)
    {
        var (subject, stop) = parameter;
        var result = await _previous.Off(parameter);

        await _complete.Off(new(result is not null ? new(subject.Address, result) : null, stop));

        return result;
    }
}