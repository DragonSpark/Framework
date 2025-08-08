using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using DeviceCheck;
using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Application.Mobile.Maui.Storage;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Results;
using Foundation;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Attestation;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<IAttestationToken>().Forward<AttestationToken>().Singleton();
    }
}

public class Attestation<T> : IAttestation<T>
{
    readonly IStopAware<string>               _challenge;
    readonly IAltering<string>                _attestation;
    readonly IStopAware<VerificationInput, T> _result;

    protected Attestation(IStopAware<string> challenge, IStopAware<VerificationInput, T> result)
        : this(challenge, AttestationToken.Default, result) {}

    protected Attestation(IStopAware<string> challenge, IAltering<string> attestation,
                          IStopAware<VerificationInput, T> result)
    {
        _challenge   = challenge;
        _attestation = attestation;
        _result      = result;
    }

    public async ValueTask<T> Get(CancellationToken parameter)
    {
        var challenge   = await _challenge.Off(parameter);
        var attestation = await _attestation.Off(challenge.Stop(parameter));
        var result      = await _result.Off(new(new(challenge, attestation), parameter));
        return result;
    }
}

sealed class AttestationToken : IAttestationToken
{
    public static AttestationToken Default { get; } = new();

    AttestationToken() : this(DCAppAttestService.SharedService, ClientKey.Default) {}

    readonly DCAppAttestService _service;
    readonly IStopAware<string> _key;

    public AttestationToken(DCAppAttestService service, IStopAware<string> key)
    {
        _service = service;
        _key     = key;
    }

    public async ValueTask<string> Get(Stop<string> parameter)
    {
        if (!_service.Supported)
        {
            throw new NotSupportedException("App Attest not supported on this device.");
        }

        var bytes       = Convert.FromBase64String(parameter);
        var hash        = SHA256.HashData(bytes);
        var data        = NSData.FromArray(hash);
        var key         = await _key.Off(parameter);
        var attestation = await _service.AttestKeyAsync(key, data).Off();
        return Convert.ToBase64String(attestation.ToArray());
    }
}

sealed class GenerateKey : IResulting<string>
{
    public static GenerateKey Default { get; } = new();

    GenerateKey() : this(DCAppAttestService.SharedService) {}

    readonly DCAppAttestService _service;

    public GenerateKey(DCAppAttestService service) => _service = service;

    public ValueTask<string> Get() => _service.GenerateKeyAsync().ToOperation();
}

sealed class ClientKey : ProcessStoring<string>
{
    public static ClientKey Default { get; } = new();

    ClientKey() : base(new Variable<string>(), ClientKeyStorage.Default) {}
}

sealed class ClientKeyStorage : DragonSpark.Application.Runtime.Objects.Storing<string>
{
    public static ClientKeyStorage Default { get; } = new();

    ClientKeyStorage() : base(ClientKeyStorageValue.Default, GenerateKey.Default.AsStop()) {}
}

sealed class ClientKeyStorageValue : StorageValue<string>
{
    public static ClientKeyStorageValue Default { get; } = new();

    ClientKeyStorageValue() : base(A.Type<ClientKeyStorageValue>().FullName.Verify()) {}
}