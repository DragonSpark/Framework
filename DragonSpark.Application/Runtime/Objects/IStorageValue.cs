using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Runtime.Objects;

public interface IStorageValue<T>
    : IStopAware<T>, IDepending, DragonSpark.Model.Operations.Results.Stop.IStopAware<T?>;

// TODO

public class Storing<T> : DragonSpark.Model.Operations.Results.Stop.IStopAware<T>
{
    readonly IStorageValue<T>           _store;
    readonly Await<CancellationToken, T> _source;

    protected Storing(IStorageValue<T> store, ISelect<CancellationToken, ValueTask<T>> source)
        : this(store, source.Off) {}

    protected Storing(IStorageValue<T> store, Await<CancellationToken, T> source)
    {
        _store  = store;
        _source = source;
    }

    public async ValueTask<T> Get(CancellationToken parameter)
    {
        var existing = await _store.Get(parameter).Off();

        if (existing is not null)
        {
            return existing;
        }

        var result = await _source(parameter);
        await _store.Off(result.Stop(parameter));
        return result;
    }
}