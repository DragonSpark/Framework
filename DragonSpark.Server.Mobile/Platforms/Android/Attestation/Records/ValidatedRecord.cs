using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation.Records;

sealed class ValidatedRecord<T> : IStopAware<NewAttestationRecordInput, T?> where T : class, IVerificationRecord
{
    readonly IValidVerification _instance;
    readonly AddRecord<T>       _add;

    public ValidatedRecord(IValidVerification instance, AddRecord<T> add)
    {
        _instance = instance;
        _add      = add;
    }

    public async ValueTask<T?> Get(Stop<NewAttestationRecordInput> parameter)
    {
        var valid  = await _instance.Off(parameter);
        var result = valid ? await _add.Off(parameter) : null;
        return result;
    }
}