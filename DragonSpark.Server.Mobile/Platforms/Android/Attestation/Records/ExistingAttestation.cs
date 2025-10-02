using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation.Records;

sealed class ExistingAttestation<T> : IExistingAttestation where T : class, IVerificationRecord
{
    readonly EvaluateRecord<T> _existing;

    public ExistingAttestation(EvaluateRecord<T> existing) => _existing = existing;

    public async ValueTask<Guid?> Get(Stop<ExistingAttestationRecordInput> parameter)
    {
        var record = await _existing.Off(parameter);
        return record?.Identity;
    }
}