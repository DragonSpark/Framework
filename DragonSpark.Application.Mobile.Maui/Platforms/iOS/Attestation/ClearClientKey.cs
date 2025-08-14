using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Application.Model.Values;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Attestation;

sealed class ClearClientKey : ClearState<string>, IClearClientKey
{
    public static ClearClientKey Default { get; } = new();

    ClearClientKey() : base(ClientKeyProcessStore.Default, ClientKeyStorageValue.Default) {}
}