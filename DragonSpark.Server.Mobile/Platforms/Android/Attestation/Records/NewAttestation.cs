using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation.Records;

sealed class NewAttestation<T> : INewAttestation where T : class, IVerificationRecord
{
    readonly ValidatedRecord<T> _add;

    public NewAttestation(ValidatedRecord<T> add) => _add = add;

    public async ValueTask<Guid?> Get(Stop<NewAttestationRecordInput> parameter)
    {
        var record = await _add.Off(parameter);
        return record?.Identity;
    }
}