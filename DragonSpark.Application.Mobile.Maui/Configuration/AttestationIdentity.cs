using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

public sealed class AttestationIdentity : ProcessStoring<ExistingAttestationResult?>, IAttestationIdentity
{
    public static AttestationIdentity Default { get; } = new();

    AttestationIdentity() : base(AttestationIdentityProcessStore.Default, AttestationIdentityStorageValue.Default) {}
}