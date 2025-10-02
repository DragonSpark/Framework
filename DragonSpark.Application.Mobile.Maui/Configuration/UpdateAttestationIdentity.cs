using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Application.Model.Values;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

sealed class UpdateAttestationIdentity : UpdateState<ExistingAttestationResult>
{
    public static UpdateAttestationIdentity Default { get; } = new();

    UpdateAttestationIdentity() : base(SaveAttestationIdentity.Default, ClearAttestationIdentity.Default) {}
}