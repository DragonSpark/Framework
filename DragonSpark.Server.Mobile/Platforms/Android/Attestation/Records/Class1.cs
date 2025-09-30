using System;
using System.ComponentModel.DataAnnotations;
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
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation.Records;

sealed class Class1;

public interface IVerificationRecord
{
    string KeyHash { get; set; } // The key identifier from the attestation
    DateTimeOffset Created { get; set; }
}

public interface IVerificationRecord<T> : IStopAware<VerificationInput, T?>;

/*public readonly record struct VerificationInput(
    string KeyHash,
    string Challenge,
    string Attestation);*/

[Index(nameof(KeyHash), IsUnique = true)]
public abstract class VerificationRecordBase : IVerificationRecord
{
    public uint Id { get; set; }

    [MaxLength(64)]
    public required string KeyHash { get; set; }

    public DateTimeOffset Created { get; set; }
}

sealed class EvaluateRecord<T> : EvaluateToSingleOrDefault<string, T> where T : class, IVerificationRecord
{
    public EvaluateRecord(IScopes scopes) : base(scopes, SelectAttestationRecord<T>.Default) {}
}

sealed class SelectAttestationRecord<T> : StartWhere<string, T> where T : class, IVerificationRecord
{
    public static SelectAttestationRecord<T> Default { get; } = new();

    SelectAttestationRecord() : base((p, x) => x.KeyHash == p) {}
}

sealed class VerificationRecord<T> : Maybe<Stop<VerificationInput>, T?>, IVerificationRecord<T>
    where T : class, IVerificationRecord
{
    public VerificationRecord(EvaluateRecord<T> existing, ValidatedRecord<T> add)
        : base(Start.A.Selection<Stop<VerificationInput>>()
                    .By.Calling(x => new Stop<string>(x.Subject.KeyHash, x))
                    .Select(existing)
                    .Out(),
               add) {}
}

sealed class ValidatedRecord<T> : IStopAware<VerificationInput, T?> where T : class, IVerificationRecord
{
    readonly IValidVerification _instance;
    readonly AddRecord<T>      _add;

    public ValidatedRecord(IValidVerification instance, AddRecord<T> add)
    {
        _instance = instance;
        _add      = add;
    }

    public async ValueTask<T?> Get(Stop<VerificationInput> parameter)
    {
        var valid  = await _instance.Off(parameter);
        var result = valid ? await _add.Off(parameter) : null;
        return result;
    }
}

sealed class AddRecord<T> : Updating<VerificationInput, T> where T : class, IVerificationRecord
{
    public AddRecord(IScopes scopes) : base(scopes, NewRecord<T>.Default) {}
}

sealed class NewRecord<T> : IStopAware<VerificationInput, T> where T : class, IVerificationRecord
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

    public ValueTask<T> Get(Stop<VerificationInput> parameter)
    {
        var (instance, _) = parameter;
        var key    = instance.KeyHash;
        var result = _new.Get();
        result.KeyHash = key;
        result.Created = _time.Get();
        return result.ToOperation();
    }
}