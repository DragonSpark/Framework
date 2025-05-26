using System.Threading.Tasks;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Communication.Http.Security;

public class UpdateTokenState : IUpdateTokenState
{
    readonly ISaveTokenState _save;
    readonly IStopAware      _clear;

    protected UpdateTokenState(ISaveTokenState save, IStopAware clear)
    {
        _save  = save;
        _clear = clear;
    }

    public ValueTask Get(Stop<AccessTokenView?> parameter)
    {
        var (subject, stop) = parameter;
        return subject is not null ? _save.Get(new(subject, stop)) : _clear.Get(stop);
    }
}