using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

sealed class EvaluateAttestationRecord<T> : EvaluateToSingleOrDefault<string, T> where T : class, IAttestationRecord
{
    public EvaluateAttestationRecord(IScopes scopes) : base(scopes, SelectAttestationRecord<T>.Default) {}
}