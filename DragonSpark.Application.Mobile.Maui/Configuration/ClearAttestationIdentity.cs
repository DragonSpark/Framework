using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Application.Model.Values;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

public sealed class ClearAttestationIdentity : ClearState<ValidationIdentityView>, IClearAttestationIdentity
{
    public static ClearAttestationIdentity Default { get; } = new();

    ClearAttestationIdentity()
        : base(ValidationIdentityProcessStore.Default, ValidationIdentityStorageValue.Default) {}
}