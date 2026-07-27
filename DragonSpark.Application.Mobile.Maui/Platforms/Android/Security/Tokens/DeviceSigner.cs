using System.Runtime.InteropServices;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences.Memory;
using Java.Security;
using Signature = Java.Security.Signature;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

sealed class DeviceSigner : IDeviceSigner
{
    public static DeviceSigner Default { get; } = new();

    DeviceSigner() : this(GetKeyStore.Default, StoreAlias.Default, "NONEwithECDSA") {}

    readonly IResult<KeyStore> _store;
    readonly string            _alias, _type;

    public DeviceSigner(IResult<KeyStore> store, string alias, string type)
    {
        _store = store;
        _alias = alias;
        _type  = type;
    }

    public ValueTask<ReadOnlyMemory<byte>> Get(Stop<ReadOnlyMemory<byte>> parameter)
    {
        var (digest, _) = parameter;

        var store = _store.Get();

        var entry = (KeyStore.PrivateKeyEntry)store.GetEntry(_alias, null).Verify();
        var key   = entry.PrivateKey;

        var signature = Signature.GetInstance(_type).Verify();
        signature.InitSign(key);

        if (MemoryMarshal.TryGetArray(digest, out var segment))
        {
            signature.Update(segment.Array, segment.Offset, segment.Count);
        }
        else
        {
            using var lease = NewLeasing<byte>.Default.Get(digest.Length);
            digest.CopyTo(lease.AsMemory());
            signature.Update(lease.Store, 0, (int)lease.Length);
        }

        var result = signature.Sign();
        return result.AsMemory().AsReadOnly().ToOperation();
    }
}