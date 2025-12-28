using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Operations.Results.Stop;

public class Storing<T> : IStopAware<T>
{
    readonly IMutationAware<T?>          _store;
    readonly Await<CancellationToken, T> _source;

    protected Storing(ISelect<CancellationToken, ValueTask<T>> source) : this(new Variable<T>(), source) {}

    protected Storing(IMutable<T?> mutable, ISelect<CancellationToken, ValueTask<T>> source)
        : this(new AssignedAwareVariable<T>(mutable), source) {}

    protected Storing(IMutationAware<T?> store, ISelect<CancellationToken, ValueTask<T>> source)
        : this(store, source.Off) {}

    protected Storing(IMutationAware<T?> store, Await<CancellationToken, T> source)
    {
        _store  = store;
        _source = source;
    }

    public async ValueTask<T> Get(CancellationToken parameter)
    {
        if (_store.IsSatisfiedBy())
        {
            return _store.Get().Verify();
        }

        var result = await _source(parameter);
        _store.Execute(result);
        return result;
    }
}