using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

sealed class AttestationIdentityProcessStore : Variable<AttestationIdentityView>
{
    public static AttestationIdentityProcessStore Default { get; } = new();

    AttestationIdentityProcessStore() {}
}