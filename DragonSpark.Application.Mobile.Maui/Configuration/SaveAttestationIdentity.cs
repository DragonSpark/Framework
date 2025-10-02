using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Application.Model.Values;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

sealed class SaveAttestationIdentity : SaveState<ExistingAttestationResult>
{
    public static SaveAttestationIdentity Default { get; } = new();

    SaveAttestationIdentity() : base(AttestationIdentityProcessStore.Default, AttestationIdentityStorageValue.Default) {}
}