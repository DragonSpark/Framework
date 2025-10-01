using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

sealed class ExistingAttestation<T> : IExistingAttestation where T : class, IAttestationRecord
{
    readonly EvaluateAttestationRecord<T> _existing;

    public ExistingAttestation(EvaluateAttestationRecord<T> existing) => _existing = existing;

    public async ValueTask<Guid?> Get(Stop<ExistingAttestationRecordInput> parameter)
    {
        var record = await _existing.Off(parameter);
        return record?.Identity;
    }
}