using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Application.Model.Values;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

public sealed class ClearAttestationIdentity : ClearState<ExistingAttestationResult>, IClearAttestationIdentity
{
    public static ClearAttestationIdentity Default { get; } = new();

    ClearAttestationIdentity()
        : base(AttestationIdentityProcessStore.Default, AttestationIdentityStorageValue.Default) {}
}