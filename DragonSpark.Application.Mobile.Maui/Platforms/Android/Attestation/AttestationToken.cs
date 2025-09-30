using System;
using System.Threading.Tasks;
using Android.Gms.Extensions;
using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.Maui.ApplicationModel;
using Xamarin.Google.Android.Play.Core.Integrity;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Attestation;

sealed class AttestationToken : IAttestationToken
{
    readonly IIntegrityManager _manager;
    readonly Request           _request;

    public AttestationToken(Request request) : this(IntegrityManagerFactory.Create(Platform.AppContext), request) {}

    public AttestationToken(IIntegrityManager manager, Request request)
    {
        _manager = manager;
        _request = request;
    }

    public async ValueTask<string> Get(Stop<string> parameter)
    {
        var request = _request.Get(parameter);
        var token   = _manager.RequestIntegrityToken(request);
        if (token is not null && await token is IntegrityTokenResponse response)
        {
            var result = response.Token();
            if (result is not null)
            {
                return result;
            }
        }
        throw new InvalidOperationException("Could not determine integrity token");
    }
}

sealed class KeyAwareAttestationToken : IAttestationToken
{
    readonly IAttestationToken     _previous;
    readonly IStorageValue<string> _key;

    public KeyAwareAttestationToken(IAttestationToken previous) : this(previous, ClientKeyStorageValue.Default) {}

    public KeyAwareAttestationToken(IAttestationToken previous, IStorageValue<string> key)
    {
        _previous = previous;
        _key      = key;
    }

    public async ValueTask<string> Get(Stop<string> parameter)
    {
        var (_, stop) = parameter;
        return await _key.Get(stop).Off() is not null
                   ? await _previous.Off(parameter)
                   : string.Empty;
    }
}