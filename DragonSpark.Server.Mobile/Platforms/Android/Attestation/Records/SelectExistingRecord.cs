using DragonSpark.Application.AspNet.Entities.Queries.Composition;

namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation.Records;

sealed class SelectExistingRecord<T> : StartWhere<ExistingAttestationRecordInput, T>
    where T : class, IVerificationRecord
{
    public static SelectExistingRecord<T> Default { get; } = new();

    SelectExistingRecord() : base((p, x) => x.Identity == p.Identity && x.KeyHash == p.KeyHash) {}
}