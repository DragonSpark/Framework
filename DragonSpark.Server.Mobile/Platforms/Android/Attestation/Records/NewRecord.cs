using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Results;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation.Records;

sealed class NewRecord<T> : IStopAware<NewAttestationRecordInput, T> where T : class, IVerificationRecord
{
    public static NewRecord<T> Default { get; } = new();

    NewRecord() : this(New<T>.Default, Time.Default) {}

    readonly IResult<T> _new;
    readonly ITime      _time;

    public NewRecord(IResult<T> @new, ITime time)
    {
        _new  = @new;
        _time = time;
    }

    public ValueTask<T> Get(Stop<NewAttestationRecordInput> parameter)
    {
        var (instance, _) = parameter;
        var key    = instance.KeyHash;
        var result = _new.Get();
        result.KeyHash  = key;
        result.Created  = _time.Get();
        result.Identity = Guid.NewGuid();
        return result.ToOperation();
    }
}