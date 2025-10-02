using DragonSpark.Application.AspNet.Entities.Queries.Composition;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

sealed class SelectAttestationRecord<T> : StartWhere<ExistingAttestationRecordInput, T> where T : class, IAttestationRecord
{
    public static SelectAttestationRecord<T> Default { get; } = new();

    SelectAttestationRecord() : base((p, x) => x.Identity == p.Identity &&  x.KeyHash == p.KeyHash) {}
}