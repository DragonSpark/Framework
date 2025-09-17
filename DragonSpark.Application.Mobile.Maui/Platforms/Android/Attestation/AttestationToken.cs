using System;
using System.Threading.Tasks;
using Android.Gms.Extensions;
using Android.Gms.Tasks;
using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.Maui.ApplicationModel;
using Xamarin.Google.Android.Play.Core.Integrity;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Attestation;

sealed class AttestationToken : IAttestationToken
{
    readonly IIntegrityManager _manager;
    readonly Request           _request;

    public AttestationToken(Request request)
        : this(IntegrityManagerFactory.Create(Platform.CurrentActivity.Verify()), request) {}

    public AttestationToken(IIntegrityManager manager, Request request)
    {
        _manager = manager;
        _request = request;
    }

    public async ValueTask<string> Get(Stop<string> parameter)
    {
        var source  = new TaskCompletionSource<string>();
        var request = _request.Get(parameter);
        var task    = _manager.RequestIntegrityToken(request).Verify();

        await task.AddOnSuccessListener(new SuccessListener(source)).AddOnFailureListener(new FailureListener(source));

        var result = await source.Task.Off();
        return result;
    }

    sealed class SuccessListener : Java.Lang.Object, IOnSuccessListener
    {
        readonly TaskCompletionSource<string> _source;

        public SuccessListener(TaskCompletionSource<string> source) => _source = source;

        public void OnSuccess(Java.Lang.Object? result)
        {
            if (result is IntegrityTokenResponse response)
            {
                var token = response.Token().Verify();
                _source.TrySetResult(token);
            }
            else
            {
                _source.TrySetException(new InvalidOperationException($"Unexpected result type: {result?.GetType()}"));
            }
        }
    }

    sealed class FailureListener : Java.Lang.Object, IOnFailureListener
    {
        readonly TaskCompletionSource<string> _source;

        public FailureListener(TaskCompletionSource<string> source) => _source = source;

        public void OnFailure(Java.Lang.Exception exception)
        {
            _source.TrySetException(exception);
        }
    }
}

// TODO

sealed class Request : ISelect<string, IntegrityTokenRequest>
{
    readonly ulong _project;

    public Request(PlayStoreVerificationSettings settings) : this(settings.Project) {}

    public Request(ulong project) => _project = project;

    public IntegrityTokenRequest Get(string parameter)
        => IntegrityTokenRequest.InvokeBuilder()
                                .Verify()
                                .SetNonce(parameter)
                                .Verify()
                                .SetCloudProjectNumber((long)_project)
                                .Verify()
                                .Build()
                                .Verify();
}

public sealed record PlayStoreVerificationSettings
{
    public ulong Project { get; set; }
}