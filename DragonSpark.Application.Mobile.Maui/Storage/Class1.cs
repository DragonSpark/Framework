using System.Threading.Tasks;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Compose;
using Microsoft.Maui.Storage;

namespace DragonSpark.Application.Mobile.Maui.Storage;

public class StorageValue<T> : IStorageValue<T>
{
    readonly string         _key;
    readonly ISerializer<T> _serializer;
    readonly ISecureStorage _storage;

    protected StorageValue() : this(A.Type<T>().FullName.Verify()) {}

    protected StorageValue(string key) : this(key, Serializer<T>.Default, SecureStorage.Default) {}

    protected StorageValue(string key, ISerializer<T> serializer, ISecureStorage storage)
    {
        _key        = key;
        _serializer = serializer;
        _storage    = storage;
    }

    public ValueTask Get(T parameter) => _storage.SetAsync(_key, _serializer.Format.Get(parameter)).ToOperation();

    public async ValueTask<T?> Get()
    {
        var storage = await _storage.GetAsync(_key).Off();
        return storage is not null ? _serializer.Parse.Get(storage) : default;
    }
}