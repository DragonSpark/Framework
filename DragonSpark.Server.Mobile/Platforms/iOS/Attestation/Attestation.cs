using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;
using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Results;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation;

public sealed record Attestation(string Format, AttestationStatement Statement, AuthenticationData AuthenticationData);

// TODO

public interface IAttestationRecord
{
    string Id { get; set; } // The key identifier from the attestation
    DateTimeOffset Created { get; set; }
    byte[] PublicKeyHash { get; set; } // SHA256 hash of the public key
    byte[] PublicKey { get; set; }     // SHA256 hash of the public key
    byte[] Receipt { get; set; }       // Receipt from the attestation statement
    uint Count { get; set; }
}

public abstract class AttestationRecordBase : IAttestationRecord
{
    [MaxLength(32)]
    public required string Id { get; set; } = null!; // The key identifier from the attestation

    public DateTimeOffset Created { get; set; }

    public required byte[] PublicKeyHash { get; set; } = null!; // SHA256 hash of the public key
    public required byte[] PublicKey { get; set; } = null!;     // SHA256 hash of the public key
    public byte[] Receipt { get; set; } = [];                   // Receipt from the attestation statement
    public uint Count { get; set; }
}

public interface IAttestationRecord<T> : IStopAware<ValidatedAttestationInput, T?>;

sealed class AttestationRecord<T> : Maybe<Stop<ValidatedAttestationInput>, T?>, IAttestationRecord<T>
    where T : class, IAttestationRecord
{
    public AttestationRecord(EvaluateAttestationRecord<T> existing, ValidatedRecord<T> add)
        : base(Start.A.Selection<Stop<ValidatedAttestationInput>>()
                    .By.Calling(x => new Stop<string>(x.Subject.KeyHash, x))
                    .Select(existing)
                    .Out(),
               add) {}
}

sealed class ValidatedRecord<T> : IStopAware<ValidatedAttestationInput, T?> where T : class, IAttestationRecord
{
    readonly ValidatedAttestation _instance;
    readonly AddRecord<T>         _add;

    public ValidatedRecord(AddRecord<T> add) : this(ValidatedAttestation.Default, add) {}

    public ValidatedRecord(ValidatedAttestation instance, AddRecord<T> add)
    {
        _instance = instance;
        _add      = add;
    }

    public async ValueTask<T?> Get(Stop<ValidatedAttestationInput> parameter)
    {
        var (input, stop) = parameter;
        var instance = _instance.Get(input);
        var result   = instance is not null ? await _add.Off(new(instance, stop)) : null;
        return result;
    }
}

sealed class AddRecord<T> : Updating<Attestation, T> where T : class, IAttestationRecord
{
    public AddRecord(IScopes scopes) : base(scopes, NewRecord<T>.Default) {}
}

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

sealed class EvaluateAttestationRecord<T> : EvaluateToSingleOrDefault<string, T> where T : class, IAttestationRecord
{
    public EvaluateAttestationRecord(IScopes scopes) : base(scopes, SelectAttestationRecord<T>.Default) {}
}

sealed class SelectAttestationRecord<T> : StartWhere<string, T> where T : class, IAttestationRecord
{
    public static SelectAttestationRecord<T> Default { get; } = new();

    SelectAttestationRecord() : base((p, x) => x.Id == p) {}
}