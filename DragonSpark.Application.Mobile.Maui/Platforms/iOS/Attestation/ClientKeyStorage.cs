using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Attestation;

sealed class ClientKeyStorage : Storing<string>
{
    public static ClientKeyStorage Default { get; } = new();

    ClientKeyStorage() : base(ClientKeyStorageValue.Default, GenerateKey.Default.AsStop()) {}
}