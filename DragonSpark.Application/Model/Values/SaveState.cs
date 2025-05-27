using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Model.Values;

public class SaveState<T> : IStopAware<T>
{
    readonly IMutable<T?>  _store;
    readonly IStopAware<T> _value;

    protected SaveState(IMutable<T?> store, IStopAware<T> value)
    {
        _store = store;
        _value = value;
    }

    public ValueTask Get(Stop<T> parameter)
    {
        var (subject, stop) = parameter;
        _store.Execute(subject);
        return _value.Get(subject.Stop(stop));
    }
}