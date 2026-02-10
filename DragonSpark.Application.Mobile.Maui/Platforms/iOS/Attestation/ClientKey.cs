using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Attestation;

sealed class ClientKey : Storing<string>, IClientKey
{
    public static ClientKey Default { get; } = new();

    public ClientKey() : base(ClientKeyProcessStore.Default, ClientKeyStorage.Default) {}
}