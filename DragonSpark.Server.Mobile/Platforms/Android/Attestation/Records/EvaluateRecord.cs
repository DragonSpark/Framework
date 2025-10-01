using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation.Records;

sealed class EvaluateRecord<T> : EvaluateToSingleOrDefault<ExistingAttestationRecordInput, T>
    where T : class, IVerificationRecord
{
    public EvaluateRecord(IScopes scopes) : base(scopes, SelectExistingRecord<T>.Default) {}
}