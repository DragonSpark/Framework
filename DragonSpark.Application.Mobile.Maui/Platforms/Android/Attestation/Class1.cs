using Android.Content;
using Android.Provider;
using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Application.Mobile.Maui.Storage;
using DragonSpark.Application.Model.Values;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Results;
using Microsoft.Maui.ApplicationModel;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Attestation;

class Class1 {}

sealed class ClientKey : ProcessStoring<string>, IClientKey
{
    public static ClientKey Default { get; } = new();

    public ClientKey() : base(ClientKeyProcessStore.Default, ClientKeyStorage.Default) {}
}

sealed class ClearClientKey : ClearState<string>, IClearClientKey
{
    public static ClearClientKey Default { get; } = new();

    ClearClientKey() : base(ClientKeyProcessStore.Default, ClientKeyStorageValue.Default) {}
}

sealed class ClientKeyProcessStore : Variable<string>
{
    public static ClientKeyProcessStore Default { get; } = new();

    ClientKeyProcessStore() {}
}

sealed class ClientKeyStorage : Storing<string>
{
    public static ClientKeyStorage Default { get; } = new();

    ClientKeyStorage() 
        : base(ClientKeyStorageValue.Default, GenerateKey.Default.Then().Operation().Out().AsStop()) {}
}

sealed class ClientKeyStorageValue : StorageValue<string>
{
    public static ClientKeyStorageValue Default { get; } = new();

    ClientKeyStorageValue() : base(A.Type<ClientKeyStorageValue>().FullName.Verify()) {}
}

sealed class GenerateKey : IResult<string>
{
    public static GenerateKey Default { get; } = new();

    GenerateKey() : this(Platform.AppContext.ContentResolver.Verify(), Settings.Secure.AndroidId) {}

    readonly ContentResolver _resolver;
    readonly string          _key;

    public GenerateKey(ContentResolver resolver, string key)
    {
        _resolver = resolver;
        _key      = key;
    }

    public string Get() => Settings.Secure.GetString(_resolver, _key).Verify();
}