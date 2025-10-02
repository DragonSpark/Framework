using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Application.Mobile.Maui.Storage;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

sealed class AttestationIdentityStorageValue : StorageValue<ExistingAttestationResult>
{
    public static AttestationIdentityStorageValue Default { get; } = new();

    AttestationIdentityStorageValue() {}
}