using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

sealed class LoadAttestation<T> : ILoadAttestation where T : class, IAttestationRecord
{
    readonly ComposeAttestationInput _input;
    readonly IAttestationRecord<T>   _record;

    public LoadAttestation(ComposeAttestationInput input, IAttestationRecord<T> record)
    {
        _input  = input;
        _record = record;
    }

    public async ValueTask<IAttestationRecord?> Get(Stop<AttestationRecordInput> parameter)
    {
        var (subject, stop) = parameter;
        var input = _input.Get(subject);
        return await _record.Off(new(input, stop));
    }
}