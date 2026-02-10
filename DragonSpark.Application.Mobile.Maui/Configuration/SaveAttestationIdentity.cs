using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Application.Model.Values;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

sealed class SaveAttestationIdentity : SaveState<ValidationIdentityView>
{
    public static SaveAttestationIdentity Default { get; } = new();

    SaveAttestationIdentity() : base(ValidationIdentityProcessStore.Default, ValidationIdentityStorageValue.Default) {}
}