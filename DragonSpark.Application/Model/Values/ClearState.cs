using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Model.Values;

public class ClearState<T> : IDepending
{
    readonly IMutable<T?> _store;
    readonly IDepending   _storage;

    protected ClearState(IMutable<T?> store, IDepending storage)
    {
        _store   = store;
        _storage = storage;
    }

    public async ValueTask<bool> Get(Stop<None> parameter)
    {
        _store.Execute(default);
        return await _storage.Get(parameter).Off();
    }
}