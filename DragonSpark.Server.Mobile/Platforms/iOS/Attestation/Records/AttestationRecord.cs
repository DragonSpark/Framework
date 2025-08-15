using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

sealed class AttestationRecord<T> : Maybe<Stop<AttestationInput>, T?>, IAttestationRecord<T>
    where T : class, IAttestationRecord
{
    public AttestationRecord(EvaluateAttestationRecord<T> existing, ValidatedRecord<T> add)
        : base(Start.A.Selection<Stop<AttestationInput>>()
                    .By.Calling(x => new Stop<string>(x.Subject.KeyHash, x))
                    .Select(existing)
                    .Out(),
               add) {}
}