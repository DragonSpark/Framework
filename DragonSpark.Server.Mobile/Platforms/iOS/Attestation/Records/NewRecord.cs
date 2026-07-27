using System.Security.Cryptography;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Results;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Activation;
using DragonSpark.Server.Mobile.Security.Devices.Authentication;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

sealed class NewRecord<T> : IStopAware<Attestation, T> where T : class, IAttestationRecord
{
    readonly CurrentDevice _current;
    readonly IResult<T>    _new;
    readonly ITime         _time;

    public NewRecord(CurrentDevice current) : this(current, New<T>.Default, Time.Default) {}

    public NewRecord(CurrentDevice current, IResult<T> @new, ITime time)
    {
        _current = current;
        _new     = @new;
        _time    = time;
    }

    public ValueTask<T> Get(Stop<Attestation> parameter)
    {
        var (instance, _)     = parameter;
        var (identifier, key) = instance.AuthenticationData.Credential.Credential;
        var result = _new.Get();
        result.KeyHash       = Convert.ToBase64String(SHA256.HashData(identifier.Value));
        result.Created       = _time.Get();
        result.Identity      = Guid.NewGuid();
        result.Count         = instance.AuthenticationData.Count;
        result.PublicKey     = key.Value;
        result.PublicKeyHash = key.Hash;
        result.Receipt       = instance.Statement.Receipt;
        result.Thumbprint    = _current.Get();
        return result.ToOperation();
    }
}