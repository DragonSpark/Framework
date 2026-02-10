using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Contracts.Security;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Communication.Http.Security;

public class PerformLoginBase : PerformLoginBase<LoginRequest>, IPerformLogin
{
    protected PerformLoginBase(IStopAware<LoginRequest, AccessTokenView?> previous, ICompleteLogin complete)
        : base(previous, complete) {}
}

public class PerformLoginBase<T> : IStopAware<T, AccessTokenView?>
    where T : Contracts.Security.LoginRequest
{
    readonly IStopAware<T, AccessTokenView?> _previous;
    readonly ICompleteLogin                  _complete;

    protected PerformLoginBase(IStopAware<T, AccessTokenView?> previous, ICompleteLogin complete)
    {
        _previous = previous;
        _complete = complete;
    }

    public async ValueTask<AccessTokenView?> Get(Stop<T> parameter)
    {
        var (_, stop) = parameter;
        var result = await _previous.Off(parameter);

        await _complete.Off(new(result, stop));

        return result;
    }
}