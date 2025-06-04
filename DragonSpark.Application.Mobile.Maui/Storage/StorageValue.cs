using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Maui.Presentation;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Storage;

namespace DragonSpark.Application.Mobile.Maui.Storage;

public class StorageValue<T> : IStorageValue<T> where T : notnull
{
    readonly string         _key;
    readonly ISerializer<T> _serializer;
    readonly ISecureStorage _storage;

    protected StorageValue() : this(A.Type<T>().FullName.Verify()) {}

    protected StorageValue(string key)
        : this(key, DefaultSerializer<T>.Default,
               CurrentServices.Default.GetService<ISecureStorage>() ?? SecureStorage.Default) {}

    protected StorageValue(string key, ISerializer<T> serializer, ISecureStorage storage)
    {
        _key        = key;
        _serializer = serializer;
        _storage    = storage;
    }

    public ValueTask Get(Stop<T> parameter) => _storage.SetAsync(_key, _serializer.Get(parameter)).ToOperation();

    public async ValueTask<T?> Get(CancellationToken _)
    {
        var storage = await _storage.GetAsync(_key).Off();
        return storage is not null ? _serializer.Parser.Get(storage) : default;
    }

    public ValueTask<bool> Get(Stop<None> parameter) => _storage.Remove(_key).ToOperation();
}