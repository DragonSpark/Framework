using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Xamarin.Google.Android.Play.Core.Integrity;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Attestation;

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