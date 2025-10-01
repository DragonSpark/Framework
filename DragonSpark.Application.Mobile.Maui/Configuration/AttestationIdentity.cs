using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Application.Mobile.Maui.Storage;
using DragonSpark.Application.Model.Values;
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

sealed class AttestationIdentityStorageValue : StorageValue<ExistingAttestationResult>
{
    public static AttestationIdentityStorageValue Default { get; } = new();

    AttestationIdentityStorageValue() {}
}
// TODO
public sealed class AttestationIdentity : ProcessStoring<ExistingAttestationResult?>, IAttestationIdentity
{
    public static AttestationIdentity Default { get; } = new();

    AttestationIdentity() : base(AttestationIdentityProcessStore.Default, AttestationIdentityStorageValue.Default) {}
}

public sealed class ClearAttestationIdentity : ClearState<ExistingAttestationResult>, IClearAttestationIdentity
{
    public static ClearAttestationIdentity Default { get; } = new();

    ClearAttestationIdentity()
        : base(AttestationIdentityProcessStore.Default, AttestationIdentityStorageValue.Default) {}
}

sealed class AttestationIdentityProcessStore : Variable<ExistingAttestationResult>
{
    public static AttestationIdentityProcessStore Default { get; } = new();

    AttestationIdentityProcessStore() {}
}