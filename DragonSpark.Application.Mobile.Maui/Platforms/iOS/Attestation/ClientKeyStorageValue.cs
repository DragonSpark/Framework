using DragonSpark.Application.Mobile.Maui.Storage;
using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Attestation;

sealed class ClientKeyStorageValue : StorageValue<string>
{
    public static ClientKeyStorageValue Default { get; } = new();

    ClientKeyStorageValue() : base(A.Type<ClientKeyStorageValue>().FullName.Verify()) {}
}