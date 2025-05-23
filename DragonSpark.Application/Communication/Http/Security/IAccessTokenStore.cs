using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Communication.Http.Security;

public interface IAccessTokenStore : DragonSpark.Model.Operations.Results.IStopAware<AccessTokenView?>;

// TODO

public class ClearTokenState : IStopAware
{
    readonly IMutable<AccessTokenView?>      _store;
    readonly IStorageValue<AccessTokenView?> _storage;

    protected ClearTokenState(IMutable<AccessTokenView?> store, IStorageValue<AccessTokenView?> storage)
    {
        _store   = store;
        _storage = storage;
    }

    public ValueTask Get(CancellationToken parameter)
    {
        _store.Execute(null);
        return _storage.Get(new(null, parameter));
    }
}