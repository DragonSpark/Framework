using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

sealed class ValidatedRecord<T> : IStopAware<AttestationInput, T?> where T : class, IAttestationRecord
{
    readonly ValidatedAttestation _instance;
    readonly AddRecord<T>         _add;

    public ValidatedRecord(AddRecord<T> add) : this(ValidatedAttestation.Default, add) {}

    public ValidatedRecord(ValidatedAttestation instance, AddRecord<T> add)
    {
        _instance = instance;
        _add      = add;
    }

    public async ValueTask<T?> Get(Stop<AttestationInput> parameter)
    {
        var (input, stop) = parameter;
        var instance = _instance.Get(input);
        var result   = instance is not null ? await _add.Off(new(instance, stop)) : null;
        return result;
    }
}