using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Google.Apis.PlayIntegrity.v1;

namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation;

sealed class ProcessIntegrityToken : IProcessIntegrityToken
{
    readonly V1Resource _api;
    readonly string     _package;

    public ProcessIntegrityToken(V1Resource api, AndroidPackageSettings settings) : this(api, settings.PackageName) {}

    public ProcessIntegrityToken(V1Resource api, string package)
    {
        _api     = api;
        _package = package;
    }

    public async ValueTask<IntegrityTokenResult> Get(Stop<string> parameter)
    {
        var (subject, _) = parameter;

        var decode   = _api.DecodeIntegrityToken(new() { IntegrityToken = subject }, _package);
        var response = await decode.ExecuteAsync().Off();
        return new(response.TokenPayloadExternal);
    }
}