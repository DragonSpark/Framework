using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Communication.Http.Security;

public class ClearTokenState : IStopAware
{
    readonly IMutable<AccessTokenView?>      _store;
    readonly IStorageValue<AccessTokenView> _storage;

    protected ClearTokenState(IMutable<AccessTokenView?> store, IStorageValue<AccessTokenView> storage)
    {
        _store   = store;
        _storage = storage;
    }

    public async ValueTask Get(CancellationToken parameter)
    {
        _store.Execute(null);
        await _storage.Get(new Stop<None>(None.Default, parameter)).Off();
    }
}