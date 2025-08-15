using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Results;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

sealed class NewRecord<T> : IStopAware<Attestation, T> where T : class, IAttestationRecord
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

    public ValueTask<T> Get(Stop<Attestation> parameter)
    {
        var (instance, _) = parameter;
        var credential = instance.AuthenticationData.Credential.Credential;
        var key        = credential.Key;
        var result     = _new.Get();
        result.Id            = Convert.ToBase64String(SHA256.HashData(credential.Identifier.Value));
        result.Created       = _time.Get();
        result.Count         = instance.AuthenticationData.Count;
        result.PublicKey     = key.Value;
        result.PublicKeyHash = key.Hash;
        result.Receipt       = instance.Statement.Receipt;
        return result.ToOperation();
    }
}