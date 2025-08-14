using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Attestation;

sealed class ClientKeyProcessStore : Variable<string>
{
    public static ClientKeyProcessStore Default { get; } = new();

    ClientKeyProcessStore() {}
}