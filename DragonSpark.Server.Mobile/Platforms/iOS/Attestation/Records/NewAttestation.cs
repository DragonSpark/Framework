using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

sealed class NewAttestation<T> : INewAttestation where T : class, IAttestationRecord
{
    readonly ComposeAttestationInput _input;
    readonly ValidatedRecord<T>      _add;

    public NewAttestation(ComposeAttestationInput input, ValidatedRecord<T> add)
    {
        _input = input;
        _add   = add;
    }

    public async ValueTask<Guid?> Get(Stop<NewAttestationRecordInput> parameter)
    {
        var (subject, stop) = parameter;
        var input  = _input.Get(subject);
        var record = await _add.Off(new(input, stop));
        return record?.Identity;
    }
}