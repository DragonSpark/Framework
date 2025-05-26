using System.Threading.Tasks;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Communication.Http.Security;

public interface ISaveTokenState : IStopAware<AccessTokenView>;
public class SaveTokenState : ISaveTokenState
{
    readonly IMutable<AccessTokenView?>     _store;
    readonly IStorageValue<AccessTokenView> _value;

    protected SaveTokenState(IMutable<AccessTokenView?> store, IStorageValue<AccessTokenView> value)
    {
        _store = store;
        _value = value;
    }

    public ValueTask Get(Stop<AccessTokenView> parameter)
    {
        var (subject, stop) = parameter;
        _store.Execute(subject);
        return _value.Get(new Stop<AccessTokenView>(subject, stop)); // TODO: Use .Stop(stop)
    }
}