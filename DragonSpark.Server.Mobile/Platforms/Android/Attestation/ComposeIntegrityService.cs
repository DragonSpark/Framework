using DragonSpark.Model.Results;
using DragonSpark.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PlayIntegrity.v1;
using Google.Apis.Services;

namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation;

sealed class ComposeIntegrityService : IResult<V1Resource>
{
    readonly string[]         _scopes;
    readonly GoogleCredential _root;

    public ComposeIntegrityService(AndroidPackageSettings settings)
        : this(GoogleCredential.FromJson(Base64Decode.Default.Get(settings.EncodedKey)),
               PlayIntegrityService.Scope.Playintegrity) {}

    public ComposeIntegrityService(GoogleCredential root, params string[] scopes)
    {
        _root   = root;
        _scopes = scopes;
    }

    public V1Resource Get()
    {
        var credential  = _root.CreateScoped(_scopes);
        var initializer = new BaseClientService.Initializer { HttpClientInitializer = credential };
        var result      = new PlayIntegrityService(initializer).V1;
        return result;
    }
}