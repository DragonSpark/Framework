using System.Threading.Tasks;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Communication.Http.Security;

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
        return _value.Get(subject.Stop(stop));
    }
}