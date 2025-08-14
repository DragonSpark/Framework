using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Attestation;

sealed class ClientKey : ProcessStoring<string>, IClientKey
{
    public static ClientKey Default { get; } = new();

    ClientKey() : base(ClientKeyProcessStore.Default, ClientKeyStorage.Default) {}
}